using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameLogic : MonoBehaviour
{
    public TextAsset daysJson;
    public TextAsset conversationsJson;
    private JObject getResult;
    private string currentDay;
    private Dictionary<string, List<bool>> currentStoryLines;
    private Dictionary<string, Day> days;
    private Dictionary<string, Dictionary<string, Conversation>> conversations;
    private Dictionary<int, string> messagesTimes;
    private WaveClicked waveClicked;
    private SectorsDeffence sectrsDeff;
    private Save savedData;
    public string dayIndex;// = "1";
    public int endingTime = 270; // ending time after 480 minutes (8 hours) pass 
    private Timer timer;
    private float susValue;
    public float healthStatusStep = 10f;
    private float susMeterValue;
    public GameObject[] sceneRadios;
    private NestedStatus statusData = new NestedStatus();

    void Start()
    {
        timer = FindObjectOfType<Timer>();
        // loads the current day, the dayIndex will be updated 
        this.days = JsonConvert.DeserializeObject<Dictionary<string, Day>>(daysJson.text);
        LoadDay();
        currentDay = dayIndex;
        conversations = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Conversation>>>(conversationsJson.text);
        LoadDayMessages(currentDay);

        waveClicked = FindObjectOfType<WaveClicked>();
        sectrsDeff = FindObjectOfType<SectorsDeffence>();
        waveClicked.setMinigames(days[currentDay].minigames);

        EnableRadios();
    }

    void Update()
    {
        if (dayIndex == "5")
        {
            Debug.Log("Ending");
            SceneManager.LoadScene("Ending");
            // return 
        }
        if (endingTime == timer.getCurrentMinutes())
        {
            EndDay();
        }

    }

    private void LoadDayMessages(string dayNum)
    {
        // Create dictionary for easy search between current time, keys and message time
        string[] messageStrings = conversations[dayNum].Keys.ToArray();
        this.messagesTimes = new Dictionary<int, string>();

        for (int i = 0; i < messageStrings.Length; i++)
        {
            this.messagesTimes.Add(timer.mmHHtoMinutes(messageStrings[i]), messageStrings[i]);
        }
    }

    public void checkMessages(int currentMinutes)
    {
        if (this.messagesTimes.ContainsKey(currentMinutes))
        {
            // if there is message at given time
            waveClicked.radioActivation(conversations[currentDay][messagesTimes[currentMinutes]]);
        }
        waveClicked.checkStopped(currentMinutes);
        sectrsDeff.CheckSectors(currentMinutes);
    }

    private void LoadDay()
    {
        var directory = new DirectoryInfo(Application.persistentDataPath);
        var files = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).Where(f => f.Name != "prefs");

        if (!files.Any())
        {  // no save was found
            FirstTimeRun();
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


        susMeterValue = savedData.susMeterValue;
        susMeterValue -= days[dayIndex].SusDecrease;  // decrease sus value daily
        currentStoryLines = savedData.storyLines;

        Dictionary<string, Day> day = JsonConvert.DeserializeObject<Dictionary<string, Day>>(daysJson.text);
        timer.SetStartingHour(day[dayIndex].startingTime);
        timer.SetEndingHour(day[dayIndex].endingTime);

        FindObjectOfType<WaveClicked>().setMinigames(day[dayIndex].minigames);
        FindObjectOfType<SusBar>().SetSusValue(susMeterValue);
    }

    private void FirstTimeRun()
    {
        dayIndex = "1";
        currentStoryLines = new Dictionary<string, List<bool>>();

        Dictionary<string, Day> day = JsonConvert.DeserializeObject<Dictionary<string, Day>>(daysJson.text);
        timer.SetStartingHour(day[dayIndex].startingTime);
        timer.SetEndingHour(day[dayIndex].endingTime);

        FindObjectOfType<WaveClicked>().setMinigames(day[dayIndex].minigames);

        // some additional setup when it is first run?
        // TODO: add reset after game is done

    }

    public void StatusFromStoryLines(string field, int amount)
    {
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
        susValue = FindObjectOfType<SusBar>().GetSusValue();
        float susDiff = susValue - susMeterValue;

        // TODO rework to loading from current status
        statusData.vehicle = EvaluateVehicleStatus(susDiff);
        statusData.health = EvaluateHealthStatus(susDiff);
        statusData.socialStatus = EvaluateSocialStatus(susDiff);
        statusData.living = EvaluateLivingStatus(susDiff);

        Save storeData = new Save();
        storeData.day = dayIndex;
        storeData.susMeterValue = susValue;
        storeData.storyLines = gameObject.GetComponent<StoryLinesLogic>().UpdateStoryLines(sectrsDeff.GetStoryLines(), currentStoryLines);
        storeData.status = statusData;

        if (PlayerPrefs.GetInt("storyLinesEnd", 0) == 1)  // if there was a storyline leading to full ending
        {
            return false;
        }

        if (days.Keys.Count.ToString() == dayIndex)   // if this was the last day of the gameplay
        {
            EvaluateGame(storeData);
            return false;
        }

        string output = JsonConvert.SerializeObject(storeData);
        System.IO.File.WriteAllText(Application.persistentDataPath + $"/saved_day-{dayIndex}.json", output);

        Debug.Log("Game succesfully saved - day" + dayIndex);
        return true;

    }

    public void EndDay()
    {
        // 1. Uloz hru 
        // 2. Prepni scenu na summary, ukaz summary
        // => V Summary je PrepareNewDay skript 
        // 3. prepni dalsi den (current day + 1) 
        //    ak je to posledny den, ukaz endgame 
        // 4. loadDays(days) zacni novy den
        if (SaveGame())
        {
            SceneManager.LoadScene("Summary");
        }
        else
        {
            SceneManager.LoadScene("Ending");
        }
        //loadDay(days);
    }

    private void EvaluateGame(Save storeData)
    {
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

    public void EnableRadios()
    {
        for (int i = 0; i < 3; i++)
        {
            sceneRadios[i].SetActive(false);
        }
        foreach (int activateRadio in days[currentDay].radiosEnabled)
        {
            sceneRadios[activateRadio - 1].tag = "Clickable";
            sceneRadios[activateRadio - 1].SetActive(true);
        }
    }

    public int[] GetDayMinigames()
    {
        //return dayMinigames;
        return days[dayIndex].minigames;
    }

    private int EvaluateHealthStatus(float susDiff)
    {
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
        float vehicleStep = 10f;
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
        float socialStatusStep = 10f;
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
        float socialLivingStep = 10f;
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
