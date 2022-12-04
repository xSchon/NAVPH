using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

class Day{
    public string StartingTime;
    public string EndingTime;
    public string StartingMessage;
    public string Scene;
    public int[] Minigames;
}

class Save{
    public string Day;
    public float SusMeterValue;
    public string[] StoryLines;
    public string Scene;
    public NestedStatus Status;
}

class NestedStatus{
    public int Vehicle;
    public int Health;
    public int SocialStatus;
    public int Living;
    
}

class Status{
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
    public string dayIndex = "1";
    public int endingTime = 480; // ending time after 480 minutes (8 hours) pass 
    //private WaveClicked waveClicked;
    private Timer timer;
    // Start is called before the first frame update
    void Start()
    {
        /*JObject getResult = JObject.Parse(jsonFile.text);
        Debug.Log(getResult.GetType());
        Debug.Log(getResult["1"]);
        Debug.Log(getResult);*/

        //waveClicked = FindObjectOfType<WaveClicked>();
        timer = FindObjectOfType<Timer>();
        //saveGame();
        loadDay(days);
        //endDay();
    }

    // Update is called once per frame
    void Update()
    {
        //timer.setEndingTime(endingTime);
        Debug.Log(timer.getCurrentMinutes());
        Debug.Log(endingTime);
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

        dayIndex = savedData.Day;       // add +1 to last loaded day

        Dictionary<string, Day> day = JsonConvert.DeserializeObject<Dictionary<string, Day>>(jsonFile.text);
        timer.setStartingTime(day[dayIndex].StartingTime);
        timer.setEndingTime(day[dayIndex].EndingTime);
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
        storeData.Scene = "LMAO";
        storeData.Status = statusData;

        string output = JsonConvert.SerializeObject(storeData);

        System.IO.File.WriteAllText(Application.persistentDataPath + $"/saved_day-{dayIndex}.json", output);

        Debug.Log("Saved brasko");
    }

    public void endDay(){
        // ukaz summary, uloz hru do json, zavolaj prepnutie current day + 1 na dalsi den, ked to bude posledny den, pusti endgame
        //saveGame();
        SceneManager.LoadScene("Summary");
    }

    private void firstTimeRun(){
        // some additional setup when it is first run?
        // TODO: add reset after game is done
        dayIndex = "1";
    }
}
