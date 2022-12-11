using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayLoader
{
    public TextAsset daysJson;
    //public TextAsset conversationsJson;
    //private JObject getResult;
    //private string currentDay;
    private Dictionary<string, Day> days;
	//private Dictionary<string, Dictionary<string, Conversation>> conversations;
    //private Dictionary<int, string> messagesTimes;
	private WaveClicked waveClicked;
    //private SectorsDeffence sectrsDeff;
    private Save savedData;
    public string dayIndex;// = "1";
    //public int endingTime = 270; // ending time after 480 minutes (8 hours) pass 
    private Timer timer;

    // public string loadDay(Dictionary<string, Day> days)
    // {
    //     var directory = new DirectoryInfo(Application.persistentDataPath);
    //     var files = directory.GetFiles().OrderByDescending(f => f.LastWriteTime);
        
    //     if (!files.Any())
    //     {          // no save was found
    //         //firstTimeRun();
    //         dayIndex = "1";
    //         return dayIndex;
    //     }
        
    //     string savedDataText = File.ReadAllText(files.First().FullName);
    //     savedData = JsonConvert.DeserializeObject<Save>(savedDataText);

    //     dayIndex = savedData.Day;    
    //     Debug.Log(dayIndex);
    //     Debug.Log(dayIndex.GetType());
    //     //increase day 
    //     int dayIndexInt = int.Parse(dayIndex);
    //     dayIndexInt++;
    //     dayIndex = dayIndexInt.ToString();
    //     Debug.Log(dayIndex);
    //     //testovacie, nebude to tu hardcoded 
    //     if (dayIndex == "5")
    //     {
    //         Debug.Log("Ending");
    //         //SceneManager.LoadScene("Ending");
    //         // return 
    //     }
    //     else {
    //     //     SceneManager.LoadScene(savedData.Scene);
    //     }

    //     Dictionary<string, Day> day = JsonConvert.DeserializeObject<Dictionary<string, Day>>(daysJson.text);
    //     timer.setStartingTime(day[dayIndex].StartingTime);
    //     timer.setEndingTime(day[dayIndex].EndingTime);
    //     //endingTime = int.Parse(day[dayIndex].EndingTime); TO DO
    //     //FindObjectOfType<WaveClicked>().setMinigames(day[dayIndex].Minigames);

    //     return dayIndex;
//}

}
