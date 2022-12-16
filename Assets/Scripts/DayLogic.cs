/* This is main script of the game. It takes care of the dayflow and gameflow
   as in loading, saving days, evaluating the results and such */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DayLogic : MonoBehaviour
{
    public TextAsset daysJson;
    public TextAsset conversationsJson;
    private Dictionary<string, List<bool>> currentStoryLines;
    private Dictionary<string, Day> days;
    private Dictionary<string, Dictionary<string, Conversation>> conversations;
    private Dictionary<int, string> messagesTimes;
    private Save savedData;
    private string dayIndex;
    private float susMeterValue;
    public GameObject[] sceneRadios;
    private NestedStatus statusData = new NestedStatus();
    public Timer timer;
    public WaveClicked waveClicked;
    public SectorsDefence sectrsDeff;
    public SusBar susBar;

    void Start()
    {
        // loads the current day, the dayIndex will be updated 
        this.days = JsonConvert.DeserializeObject<Dictionary<string, Day>>(daysJson.text);
        LoadDay();

        // load conversations, minigames and radios of given day
        conversations = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Conversation>>>(conversationsJson.text);
        LoadDayMessages();
        waveClicked.setMinigames(days[dayIndex].minigamesEnabled);
        EnableRadios();
    }

    public void EnableRadios()
    {
        // enable only those radios that are supposed to be used on certain day
        for (int i = 0; i < 3; i++)
        {
            sceneRadios[i].SetActive(false);
        }
        foreach (int activateRadio in days[dayIndex].radiosEnabled)
        {
            sceneRadios[activateRadio - 1].tag = "Clickable";
            sceneRadios[activateRadio - 1].SetActive(true);
        }
    }

    private void LoadDayMessages()
    {
        // Create dictionary for easy search between current time, keys and message time
        string[] messageStrings = conversations[dayIndex].Keys.ToArray();
        this.messagesTimes = new Dictionary<int, string>();

        for (int i = 0; i < messageStrings.Length; i++)
        {
            this.messagesTimes.Add(timer.mmHHtoMinutes(messageStrings[i]), messageStrings[i]);
        }
    }

    public void CheckMessages(int currentMinutes)
    {
        // check if there is a message for given time in loaded messsages
        if (this.messagesTimes.ContainsKey(currentMinutes))
        {
            // if yes, display this message
            waveClicked.radioActivation(conversations[dayIndex][messagesTimes[currentMinutes]]);
        }
        waveClicked.checkStopped(currentMinutes);
        sectrsDeff.CheckSectors(currentMinutes);
    }

    private void LoadDay()
    {
        DirectoryInfo directory = new DirectoryInfo(Application.persistentDataPath);
        IEnumerable<FileInfo> files = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).Where(f => f.Name != "prefs");

        if (!files.Any())
        {  // no save was found
            dayIndex = "1";
            currentStoryLines = new Dictionary<string, List<bool>>();
            SetupDay();
            return;
        }

        string savedDataText = File.ReadAllText(files.First().FullName);
        savedData = JsonConvert.DeserializeObject<Save>(savedDataText);

        dayIndex = savedData.day;
        //increase day 
        int dayIndexInt = int.Parse(dayIndex);
        dayIndexInt++;
        dayIndex = dayIndexInt.ToString();
        Debug.Log("Today is the day number: " + dayIndex);

        susMeterValue = savedData.susMeterValue;  // susMeter from save

        int decreaseAmount = days[dayIndex].susDecrease;  // decrease sus value daily, based on dificulty as well
        int difficulty = PlayerPrefs.GetInt("difficulty", 2);
        if (difficulty == 1) { decreaseAmount = (int)(decreaseAmount * 1.5); } // easy
        if (difficulty == 3) { decreaseAmount = (int)(decreaseAmount * 0.5); } // hard
        susMeterValue -= decreaseAmount;

        susBar.SetSusValue(susMeterValue);
        SetupDay();
        currentStoryLines = savedData.storyLines;
    }

    private void SetupDay()
    {
        // load information about current day and fill them into other scripts
        Dictionary<string, Day> day = JsonConvert.DeserializeObject<Dictionary<string, Day>>(daysJson.text);
        timer.SetStartingHour(day[dayIndex].startingTime);
        timer.SetEndingHour(day[dayIndex].endingTime);

        waveClicked.setMinigames(day[dayIndex].minigamesEnabled);
    }

    public void StatusFromStoryLines(string field, int amount)
    {
        // If storylines influence the status before saving, do it here - with the check as well
        switch (field)
        {
            case "None":
                return;
            case "vehicle":
                statusData.vehicle += amount;
                if (statusData.vehicle > 3) statusData.vehicle = 3;
                if (statusData.vehicle < 0) statusData.vehicle = 0;
                break;
            case "health":
                statusData.health += amount;
                if (statusData.health > 3) statusData.health = 3;
                if (statusData.health < 0) statusData.health = 0;
                break;
            case "socialStatus":
                statusData.socialStatus += amount;
                if (statusData.socialStatus > 3) statusData.socialStatus = 3;
                if (statusData.socialStatus < 0) statusData.socialStatus = 0;
                break;
            case "living":
                statusData.vehicle += amount;
                if (statusData.socialStatus > 3) statusData.socialStatus = 3;
                if (statusData.socialStatus < 0) statusData.socialStatus = 0;
                break;
        }
    }

    private bool SaveGame()
    {
        float newSusValue = susBar.GetSusValue();
        float susDiff = newSusValue - susMeterValue;

        statusData.vehicle = EvaluateVehicleStatus(susDiff);
        statusData.health = EvaluateHealthStatus(susDiff);
        statusData.socialStatus = EvaluateSocialStatus(susDiff);
        statusData.living = EvaluateLivingStatus(susDiff);

        Save storeData = new Save();
        storeData.day = dayIndex;
        storeData.susMeterValue = newSusValue;
        storeData.storyLines = gameObject.GetComponent<StoryLinesLogic>().UpdateStoryLines(sectrsDeff.GetStoryLines(), currentStoryLines);
        storeData.status = statusData;

        if (PlayerPrefs.GetInt("storyLinesEnd", 0) == 1)  // if there was a storyline leading to full ending, end the game
        {
            return false;
        }

        if (days.Keys.Count.ToString() == dayIndex)   // if this was the last day of the gameplay, end the game
        {
            EvaluateGame(storeData);
            return false;
        }

        // Save game file on disk
        string output = JsonConvert.SerializeObject(storeData);
        System.IO.File.WriteAllText(Application.persistentDataPath + $"/saved_day-{dayIndex}.json", output);

        Debug.Log("Game succesfully saved - day" + dayIndex);
        return true;
    }

    public void EndDay()
    {
        if (SaveGame())
        {
            SceneManager.LoadScene("Summary");
        }
        else
        {
            SceneManager.LoadScene("Ending");
        }
    }

    private void EvaluateGame(Save storeData)
    {
        // different endings based on how player performed during their journey
        Debug.Log("You have finished the game!");
        string endingText = "";
        endingText += "Thank you for your service.\n\n";

        int totalPassed = 0;
        foreach (var eval in storeData.storyLines.Values.ToList())
        {
            totalPassed += eval.Where(c => c).Count();
        }

        if (totalPassed < 6 && storeData.susMeterValue < 30)
        {
            endingText += "You have defended your country well. Very few people escaped, there was no room for revolution. CSSR will be under the regime for long years to come, communists won't leave it and life will be hard. \n Was it really a good job?";
        }
        else if (totalPassed < 10 && storeData.susMeterValue < 50)
        {
            endingText += "You did well, but some people escaped regardless. The regime in ČSSR will last for few decades, but will be weaker and weaker. In the end, our country will be free.";
        }
        else
        {
            endingText += "You were close to being caught, but you helped many good people. You will be remembered for being on the good side of the history. More people will escape, occupation will not last very long. Well done.";
        }
        PlayerPrefs.SetString("endingText", endingText);
        PlayerPrefs.Save();
    }

    private int EvaluateHealthStatus(float susDiff)
    {
        float healthStatusStep = 15f;
        int currentStatus = 3;

        if (savedData != null)
            currentStatus = savedData.status.health;

        if ((susDiff >= healthStatusStep) & (currentStatus != 0))
            return currentStatus - 1;
        else if ((susDiff < healthStatusStep) && (currentStatus != 3))
            return currentStatus + 1;
        else
            return currentStatus;
    }

    private int EvaluateVehicleStatus(float susDiff)
    {
        float vehicleStep = 15f;
        int currentStatus = 3;

        if (savedData != null)
            currentStatus = savedData.status.vehicle;

        if ((susDiff >= vehicleStep) & (currentStatus != 0))
            return currentStatus - 1;
        else if ((susDiff < vehicleStep) && (currentStatus != 3))
            return currentStatus + 1;
        else
            return currentStatus;
    }

    private int EvaluateSocialStatus(float susDiff)
    {
        float socialStatusStep = 15f;
        int currentStatus = 3;

        if (savedData != null)
            currentStatus = savedData.status.socialStatus;

        if ((susDiff >= socialStatusStep) & (currentStatus != 0))
            return currentStatus - 1;
        else if ((susDiff < socialStatusStep) && (currentStatus != 3))
            return currentStatus + 1;
        else
            return currentStatus;
    }

    private int EvaluateLivingStatus(float susDiff)
    {
        float socialLivingStep = 15f;
        int currentStatus = 3;

        if (savedData != null)
            currentStatus = savedData.status.living;

        if ((susDiff >= socialLivingStep) & (currentStatus != 0))
            return currentStatus - 1;
        else if ((susDiff < socialLivingStep) && (currentStatus != 3))
            return currentStatus + 1;
        else
            return currentStatus;
    }
}
