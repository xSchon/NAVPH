/* Load and display letters from your supervisors at the start of the day. */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine;
using TMPro;


public class DayStartLetters : MonoBehaviour
{
    private Dictionary<string, Dictionary<string, Dictionary<string, string>>> dailyMessages;
    public string dayIndex;
    public TextAsset jsonFile;
    [SerializeField] private TMPro.TextMeshProUGUI letterTextMesh;

    void Start()
    {
        DirectoryInfo directory = new DirectoryInfo(Application.persistentDataPath);
        IEnumerable<FileInfo> files = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).Where(f => f.Name != "prefs");

        if (!files.Any())
        {
            // no save was found, create new
            dayIndex = "1"; 
        }
        else  
        {
            // load day number from saved files
            Save savedData = JsonConvert.DeserializeObject<Save>(File.ReadAllText(files.First().FullName));
            dayIndex = savedData.day;

            int dayIndexInt = int.Parse(dayIndex) + 1;
            dayIndex = dayIndexInt.ToString();
        }

        // load new day message accordingly to day
        dailyMessages = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(jsonFile.text);
        letterTextMesh.text = dailyMessages[dayIndex]["Morning"]["Text1"];
    }
}
