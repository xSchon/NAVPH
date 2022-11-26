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

public class GameLogic : MonoBehaviour
{
    public TextAsset jsonFile;
    private JObject getResult;
    private int[] playerSectors;
    private Dictionary<string, Day> days;
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

        loadDay(days);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(getResult["1"]);
    }
    
    public void setSectors(int[] sectors){
        this.playerSectors = sectors;
        Debug.Log("New sectors are "+ this.playerSectors);
    }

    private void loadDay(Dictionary<string, Day> days){
        timer.setStartingTime(days[dayIndex].StartingTime);
        timer.setEndingTime(days[dayIndex].EndingTime);

        FindObjectOfType<WaveClicked>().setMinigames(days[dayIndex].Minigames);
    }

}
