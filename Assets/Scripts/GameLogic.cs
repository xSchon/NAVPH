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


public class GameLogic : MonoBehaviour
{
    public TextAsset daysJson;
    public TextAsset conversationsJson;
    private JObject getResult;
    private string currentDay;
    private Dictionary<string, Day> days;
    private Dictionary<string, Dictionary<string, Conversation>> conversations;
    private Dictionary<int, string> messagesTimes;
    private Timer timer;

    private WaveClicked waveClicked;
    private sectorsDeffence sectrsDeff;

    // Start is called before the first frame update
    void Start()
    {
        // TODO ADD LOADING CURRENT DAY VIA SAVES.JSON
        currentDay = "1";
        days = JsonConvert.DeserializeObject<Dictionary<string, Day>>(daysJson.text);
        conversations = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Conversation>>>(conversationsJson.text);
        timer = GameObject.Find("DailyTimer").GetComponent<Timer>();
        loadDayMessages(currentDay);

        waveClicked = FindObjectOfType<WaveClicked>();
        sectrsDeff = FindObjectOfType<sectorsDeffence>();
        waveClicked.setMinigames(days[currentDay].Minigames);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void loadDayMessages(string dayNum){
        // Create dictionary for easy search between current time, keys and message time
        string [] messageStrings = conversations[dayNum].Keys.ToArray();
        this.messagesTimes = new Dictionary<int, string>();

        for (int i = 0; i < messageStrings.Length; i++){
            this.messagesTimes.Add(timer.mmHHtoMinutes(messageStrings[i]), messageStrings[i]);
        }
    }
    
    public void checkMessages(int currentMinutes){
        if (this.messagesTimes.ContainsKey(currentMinutes)){ 
            // if there is message at given time
            waveClicked.radioActivation(conversations[currentDay][messagesTimes[currentMinutes]]);
        }
        waveClicked.checkStopped(currentMinutes);
        sectrsDeff.CheckSectors(currentMinutes);
    }

}
