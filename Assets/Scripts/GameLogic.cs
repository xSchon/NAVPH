using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

class Save
{
    public string Day;
    public float SusMeterValue;
    public Dictionary<string, List<bool>> StoryLines = new Dictionary<string, List<bool>>();
    public NestedStatus Status;
}

class NestedStatus
{
    public int Vehicle;
    public int Health;
    public int SocialStatus;
    public int Living;
    
}

class Status
{
    public string[] Vehicle;
    public string[] Health;
    public string[] SocialStatus;
    public string[] Living;
    
}

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
    private float susMeterValue;
    public GameObject[] sceneRadios;

    void Start()
    {
        //Debug.Log(Application.persistentDataPath);

        timer = FindObjectOfType<Timer>();
        // loads the current day, the dayIndex will be updated 
        loadDay(days);
        currentDay = dayIndex;
		days = JsonConvert.DeserializeObject<Dictionary<string, Day>>(daysJson.text);
        conversations = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Conversation>>>(conversationsJson.text);
        loadDayMessages(currentDay);

        waveClicked = FindObjectOfType<WaveClicked>();
        sectrsDeff = FindObjectOfType<SectorsDeffence>();
        waveClicked.setMinigames(days[currentDay].minigames);
    }

    void Update()
    {
        //timer.setEndingTime(endingTime);
        //Debug.Log(timer.getCurrentMinutes());
        //Debug.Log(endingTime);
        if (dayIndex == "5")
        {
            Debug.Log("Ending");
            SceneManager.LoadScene("Ending");
            // return 
       }
        if (endingTime == timer.getCurrentMinutes())
        {
            //Debug.Log("End of day");
            endDay();
        }

    }   

    private void loadDayMessages(string dayNum)
    {
        Debug.Log("DAY MESSAGES LOADED");
        // Create dictionary for easy search between current time, keys and message time
        string [] messageStrings = conversations[dayNum].Keys.ToArray();
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



    private void loadDay(Dictionary<string, Day> days)
    {
        var directory = new DirectoryInfo(Application.persistentDataPath);
        var files = directory.GetFiles().OrderByDescending(f => f.LastWriteTime);
        
        if (!files.Any() || ((files.Count() == 1) && (files.First().Name == "prefs")))
        {  // no save was found
            firstTimeRun();
            return;
        }
        
        string savedDataText = File.ReadAllText(files.First().FullName);
        savedData = JsonConvert.DeserializeObject<Save>(savedDataText);

        dayIndex = savedData.Day;    
        susMeterValue = savedData.SusMeterValue;
        currentStoryLines = savedData.StoryLines;

        //increase day 
        int dayIndexInt = int.Parse(dayIndex);
        dayIndexInt++;
        dayIndex = dayIndexInt.ToString();
        Debug.Log(dayIndex);
        //testovacie, nebude to tu hardcoded 
        if (dayIndex == "6")
        {
            Debug.Log("Ending");
            SceneManager.LoadScene("Ending");
            // return 
       }
       else {
       //     SceneManager.LoadScene(savedData.Scene);
        }

        Dictionary<string, Day> day = JsonConvert.DeserializeObject<Dictionary<string, Day>>(daysJson.text);
        timer.setStartingTime(day[dayIndex].startingTime);
        timer.setEndingTime(day[dayIndex].endingTime);
        //endingTime = int.Parse(day[dayIndex].EndingTime); TO DO
        FindObjectOfType<WaveClicked>().setMinigames(day[dayIndex].minigames);
        FindObjectOfType<SusBar>().setSusValue(susMeterValue);
    }


    private void SaveGame()
    {
        NestedStatus statusData = new NestedStatus();
        statusData.Vehicle = 1;
        statusData.Health = 1;
        statusData.SocialStatus = 1;
        statusData.Living = 1;
        
        Save storeData = new Save();
        storeData.Day = dayIndex;
        susValue = FindObjectOfType<SusBar>().getSusValue();
        storeData.SusMeterValue = susValue;
        storeData.StoryLines = gameObject.GetComponent<StoryLinesLogic>().UpdateStoryLines(sectrsDeff.GetStoryLines(), currentStoryLines);
        storeData.Status = statusData;

        string output = JsonConvert.SerializeObject(storeData);
        System.IO.File.WriteAllText(Application.persistentDataPath + $"/saved_day-{dayIndex}.json", output);

        Debug.Log("Game succesfully saved - day"+dayIndex);
    }

    public void endDay()
    {
        // 1. Uloz hru 
        // 2. Prepni scenu na summary, ukaz summary
        // => V Summary je PrepareNewDay skript 
        // 3. prepni dalsi den (current day + 1) 
        //    ak je to posledny den, ukaz endgame 
        // 4. loadDays(days) zacni novy den
        SaveGame();
        SceneManager.LoadScene("Summary");
        
        
        //loadDay(days);
    }

    private void firstTimeRun(){
        currentStoryLines = new Dictionary<string, List<bool>>();
        // some additional setup when it is first run?
        // TODO: add reset after game is done
        dayIndex = "1";
    }
}
