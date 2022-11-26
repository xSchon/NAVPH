using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine;

class Day{
    public string StartingTime = "8:00";
    public string EndingTime = "16:00";
    public string StartingMessage = "Welcome to another day in your work"; 
    public string Scene;
    public int[] Minigames;
}

class Conversation{
    public string Name = "Anonymous";
    public string HourShow;
    public string HourTill;
    public string[] Text;
    public int StoryLine = 0;
    public int Sector = 0;
    public string WhenDeffendSector;
    public int SusMeterPenalisation = 10;
}

public class GameLogic : MonoBehaviour
{
    public TextAsset daysJson;
    public TextAsset conversationsJson;
    private JObject getResult;
    private string currentDay;
    private int[] playerSectors;
    private Dictionary<string, Day> days;
    private Dictionary<string, Dictionary<string, Conversation>> conversations;
    private Dictionary<int, string> messagesTimes;
    private Timer timer;

    private WaveClicked waveClicked;
    // Start is called before the first frame update
    void Start()
    {
        // TODO ADD LOADING CURRENT DAY VIA SAVES.JSON
        currentDay = "1";
        days = JsonConvert.DeserializeObject<Dictionary<string, Day>>(daysJson.text);
        conversations = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Conversation>>>(conversationsJson.text);
        timer = GameObject.Find("DailyTimer").GetComponent<Timer>();
        loadDayMessages(currentDay);

        FindObjectOfType<WaveClicked>().setMinigames(days[currentDay].Minigames);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void setSectors(int[] sectors){
        this.playerSectors = sectors;
        Debug.Log("New sectors are "+ this.playerSectors);
    }

    private void loadDayMessages(string dayNum){
        // Create dictionary for easy search between current time, keys and message time
        string [] messageStrings = conversations[dayNum].Keys.ToArray();
        this.messagesTimes = new Dictionary<int, string>();

        for (int i = 0; i < messageStrings.Length; i++){
            this.messagesTimes.Add(timer.mmHHtoMinutes(messageStrings[i]), messageStrings[i]);
        }
    }
    
    public void checkMessages(int currentMintes){
        if (this.messagesTimes.Keys.ToArray().Contains(currentMintes)){ 
            // if there is message at given time
            Debug.Log(conversations[currentDay][messagesTimes[currentMintes]].Text[0]);
        }
    }

}
