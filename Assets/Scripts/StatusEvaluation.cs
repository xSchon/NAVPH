/* Script calculating state of players status based on saves and status.json */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using UnityEngine;
using TMPro;

public class StatusEvaluation : MonoBehaviour
{
    public TextAsset jsonFile;
    public TextAsset statusFile;
    public TextAsset saveJsonFile;
    public TMP_Text news;
    private Save savedData;
    private Dictionary<string, Dictionary<string, Dictionary<string, string>>> dailyMessages;
    private List<string> newspaperTexts = new List<string>();
    public List<TMPro.TextMeshProUGUI> newspaperTextMeshes = new List<TMPro.TextMeshProUGUI>();

    void Start()
    {
        LoadDayStatus();

        DirectoryInfo directory = new DirectoryInfo(Application.persistentDataPath);
        IEnumerable<FileInfo> files = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).Where(f => f.Name != "prefs");
        string savedDataText = File.ReadAllText(files.First().FullName);
        savedData = JsonConvert.DeserializeObject<Save>(savedDataText);

        dailyMessages = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(jsonFile.text);
        for (int i = 1; i <= 6; i++)
        {
            string newspaperText = dailyMessages[savedData.day]["Evening"]["Text" + i.ToString()];
            newspaperTexts.Add(newspaperText);
        }

        // This is used if any side-quest storyLine was completed
        if (PlayerPrefs.GetString("storyText1", "") != "")
        {
            newspaperTexts[index: 1] = PlayerPrefs.GetString("storyText1", "");
        }
        if (PlayerPrefs.GetString("storyText2", "") != "")
        {
            newspaperTexts[index: 2] = PlayerPrefs.GetString("storyText2", "");
        }

        for (int i = 0; i < newspaperTexts.Count; i++)
        {
            newspaperTextMeshes[i].text = newspaperTexts[i];
        }
    }

    private void LoadDayStatus()
    {
        // load all files in save directory
        DirectoryInfo directory = new DirectoryInfo(Application.persistentDataPath);
        IEnumerable<FileInfo> files = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).Where(f => f.Name != "prefs");

        // load data about status mapping save to status.json 
        string savedDataText = File.ReadAllText(directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First().FullName);
        Save savedData = JsonConvert.DeserializeObject<Save>(savedDataText);
        NestedStatus nestedStatus = savedData.status;
        Status newStatus = JsonConvert.DeserializeObject<Status>(statusFile.text);

        news.text = "";
        news.text = "Vehicle: " + newStatus.vehicle[nestedStatus.vehicle] + "\n" +
                    "Health: " + newStatus.health[nestedStatus.health] + "\n" +
                    "Social Status: " + newStatus.socialStatus[nestedStatus.socialStatus] + "\n" +
                    "Living: " + newStatus.living[nestedStatus.living];
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("OfficeScene");
    }
}