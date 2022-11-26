using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine;

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
    public Status Status;
}

class Status{
    public int Vehicle;
    public int Health;
    public int SocialStatus;
    public int Living;
    
}

public class GameLogic : MonoBehaviour
{
    public TextAsset jsonFile;
    private JObject getResult;
    private int[] playerSectors;
    private Dictionary<string, Day> days;
    private Save savedData;
    public string dayIndex = "1";
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

        days = JsonConvert.DeserializeObject<Dictionary<string, Day>>(jsonFile.text);

        Debug.Log(days["1"]);
        Debug.Log(days["1"].StartingTime);

        //FindObjectOfType<WaveClicked>().setMinigames(days["1"].Minigames);
        timer = FindObjectOfType<Timer>();

        //saveGame();
        loadDay(days);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(getResult["1"]);
    }
    
    public void setSectors(int[] sectors){
        this.playerSectors = sectors;
    }

    private void loadDay(Dictionary<string, Day> days){
        timer.setStartingTime(days[dayIndex].StartingTime);
        timer.setEndingTime(days[dayIndex].EndingTime);

        FindObjectOfType<WaveClicked>().setMinigames(days[dayIndex].Minigames);

        string savedDataText = File.ReadAllText(Application.persistentDataPath + $"/saved_day-{dayIndex}.json");
        savedData = JsonConvert.DeserializeObject<Save>(savedDataText);
        Debug.Log(savedData.Scene);
    }

    private void saveGame(){
        Status statusData = new Status();
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
}
