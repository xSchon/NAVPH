using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

class Day
{
    public string StartingTime;
    public string EndingTime;
    public string StartingMessage;
    public string Scene;
    public int[] Minigames;
}

class Save
{
    public string Day;
    public float SusMeterValue;
    public string[] StoryLines;
    public string Scene;
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
    public TextAsset jsonFile;
    private JObject getResult;
    private int[] playerSectors;
    private Dictionary<string, Day> days;
    private Save savedData;
    public string dayIndex;// = "1";
    public int endingTime;// = 480; // ending time after 480 minutes (8 hours) pass 
    //private WaveClicked waveClicked;
    private Timer timer;

    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        /*JObject getResult = JObject.Parse(jsonFile.text);
        Debug.Log(getResult.GetType());
        Debug.Log(getResult["1"]);
        Debug.Log(getResult);*/

        //waveClicked = FindObjectOfType<WaveClicked>();
        timer = FindObjectOfType<Timer>();
        //saveGame();
        loadDay(days);
        Debug.Log(days);
        //endDay();
    }

    // Update is called once per frame
    void Update()
    {
        //timer.setEndingTime(endingTime);
        //Debug.Log(timer.getCurrentMinutes());
        //Debug.Log(endingTime);
        if (endingTime == timer.getCurrentMinutes())
        {
            Debug.Log("End of day");
            endDay();
        }

    }
    
    public void setSectors(int[] sectors){
        this.playerSectors = sectors;
    }

    private void loadDay(Dictionary<string, Day> days){
        var directory = new DirectoryInfo(Application.persistentDataPath);
        var files = directory.GetFiles().OrderByDescending(f => f.LastWriteTime);
        
        if (!files.Any()){          // no save was found
            firstTimeRun();
            return;
        }
        
        string savedDataText = File.ReadAllText(files.First().FullName);
        savedData = JsonConvert.DeserializeObject<Save>(savedDataText);

        dayIndex = savedData.Day;    
        //increase day 
        int dayIndexInt = int.Parse(dayIndex);
        dayIndexInt++;
        dayIndex = dayIndexInt.ToString();
        Debug.Log(dayIndex);
        //testovacie, nebude to tu hardcoded 
        if (dayIndex == "5")
        {
            Debug.Log("Ending");
            //SceneManager.LoadScene("Ending");
            // return 
       }
       else {
       //     SceneManager.LoadScene(savedData.Scene);
        }

        Dictionary<string, Day> day = JsonConvert.DeserializeObject<Dictionary<string, Day>>(jsonFile.text);
        timer.setStartingTime(day[dayIndex].StartingTime);
        timer.setEndingTime(day[dayIndex].EndingTime);
        //endingTime = int.Parse(day[dayIndex].EndingTime); TO DO
        FindObjectOfType<WaveClicked>().setMinigames(day[dayIndex].Minigames);
    }


    private void saveGame(){
        NestedStatus statusData = new NestedStatus();
        statusData.Vehicle = 1;
        statusData.Health = 1;
        statusData.SocialStatus = 1;
        statusData.Living = 1;
        
        Save storeData = new Save();
        storeData.Day = dayIndex;
        storeData.SusMeterValue = 0.45f;
        storeData.StoryLines = new string[] {"lalala", "xdxdxdxd", "more", "gadzo"};
        storeData.Scene = "SampleScene";
        storeData.Status = statusData;

        string output = JsonConvert.SerializeObject(storeData);

        System.IO.File.WriteAllText(Application.persistentDataPath + $"/saved_day-{dayIndex}.json", output);

        Debug.Log("Saved brasko");
    }

    public void endDay()
    {
        // 1. Uloz hru 
        // 2. Prepni scenu na summary, ukaz summary
        // => V Summary je PrepareNewDay skript 
        // 3. prepni dalsi den (current day + 1) 
        //    ak je to posledny den, ukaz endgame 
        // 4. loadDays(days) zacni novy den

        saveGame();
        SceneManager.LoadScene("Summary");
        //loadDay(days);
    }

    private void firstTimeRun(){
        // some additional setup when it is first run?
        // TODO: add reset after game is done
        dayIndex = "1";
    }
}
