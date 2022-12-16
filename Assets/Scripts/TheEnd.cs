/* Loading messages and status for the Ending screen based on the ending achieved */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using UnityEngine;
using TMPro;

public class TheEnd : MonoBehaviour
{
    public TextAsset statusFile;
    public TMPro.TextMeshProUGUI summaryTextMesh;
    public TMPro.TextMeshProUGUI statusTextMesh;
    private string endingText;

    void Start()
    {
        // Messages are passed from Office scene via playerPrefs based on achieved ending
        endingText = PlayerPrefs.GetString("endingText", "");
        endingText += "\n \n Thank you for playing.";
        summaryTextMesh.text = endingText;

        statusTextMesh.text = GetStatusString();
    }

    public string GetStatusString()
    {
        DirectoryInfo directory = new DirectoryInfo(Application.persistentDataPath);
        IEnumerable<FileInfo> files = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).Where(f => f.Name != "prefs");
        Status newStatus = JsonConvert.DeserializeObject<Status>(statusFile.text);

        if (!files.Any())
        // return basic status - the best one, if there are not save files
        {
            return "Vehicle: " + newStatus.vehicle[newStatus.vehicle.Length - 1] + "\n" +
                    "Health: " + newStatus.health[newStatus.health.Length - 1] + "\n" +
                    "Social Status: " + newStatus.socialStatus[newStatus.socialStatus.Length - 1] + "\n" +
                    "Living: " + newStatus.living[newStatus.living.Length - 1] + "\n";
        }

        string savedDataText = File.ReadAllText(directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First().FullName);
        Save savedData = JsonConvert.DeserializeObject<Save>(savedDataText);
        NestedStatus nestedStatus = savedData.status;
        // otherwise return status based on given day
        return "Vehicle: " + newStatus.vehicle[nestedStatus.vehicle] + "\n" +
                "Health: " + newStatus.health[nestedStatus.health] + "\n" +
                "Social Status: " + newStatus.socialStatus[nestedStatus.socialStatus] + "\n" +
                "Living: " + newStatus.living[nestedStatus.living] + "\n";
    }

    public void Return()
    {
        SceneManager.LoadScene("Menu");
    }
}
