using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using UnityEngine;
using TMPro;

public class SummaryEvaluation : MonoBehaviour
{
    public TextAsset jsonFile;
    public TextAsset statusFile;
    public TextAsset saveJsonFile;
    public TMP_Text news;
    private Save savedData;
    private Dictionary <string, Dictionary<string, Dictionary<string, string>>> dailyMessages;
    public string dayIndex;
    private List<string> newspaperTexts = new List<string>();
    //[SerializeField] private TMPro.TextMeshProUGUI newspaperText1Mesh;
    [SerializeField] private List<TMPro.TextMeshProUGUI> newspaperTextMeshes = new List<TMPro.TextMeshProUGUI>();

    void Start()
    {
        LoadDay();
        var directory = new DirectoryInfo(Application.persistentDataPath);
        var files = directory.GetFiles().OrderByDescending(f => f.LastWriteTime);
        if (!files.Any())
        {      
               dayIndex = "1"; // no save was found
        }
        else // ak uz existuje save, nacitaj ho 
        {
            string savedDataText = File.ReadAllText(files.First().FullName);
            savedData = JsonConvert.DeserializeObject<Save>(savedDataText);
            dayIndex = savedData.day;
            // prejdi na dalsi den 
            int dayIndexInt = int.Parse(dayIndex);
//             dayIndexInt++;
//             dayIndex = dayIndexInt.ToString();
            
//             Debug.Log(dayIndex);
//         // testovacie, nebude to tu hardcoded 
//             if (dayIndex == "5")
//             {
//                 Debug.Log("Ending");
//                 //SceneManager.LoadScene("Ending");
//                 return;
//             }
//             else 
//             {
// //
//             }
            //Debug.Log(dayIndex);
        }

        dailyMessages = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(jsonFile.text);
        for (int i = 1; i <= 6; i++)
        {
            string newspaperText = dailyMessages[dayIndex]["Evening"]["Text" + i.ToString()];
            newspaperTexts.Add(newspaperText);
        }

        for (int i = 0; i < newspaperTexts.Count; i++)
        {
            newspaperTextMeshes[i].text = newspaperTexts[i];
        }


        //newspaperText1 = dailyMessages[dayIndex]["Evening"]["Text1"];
        //newspaperText1Mesh.text = newspaperText1;
    }

    private void LoadDay(){
        var directory = new DirectoryInfo(Application.persistentDataPath);
        var files = directory.GetFiles().OrderByDescending(f => f.LastWriteTime);
        
        Status newStatus = JsonConvert.DeserializeObject<Status>(statusFile.text);

        if (!files.Any() || ((files.Count() == 1) && (files.First().Name == "prefs")))
        {
            news.text = "";
            news.text = newStatus.vehicle[newStatus.vehicle.Length - 1] + "\n" 
                    + newStatus.health[newStatus.health.Length - 1] + "\n"
                    + newStatus.socialStatus[newStatus.socialStatus.Length - 1] + "\n"
                    + newStatus.living[newStatus.living.Length - 1];
            return;
        }

        string savedDataText = File.ReadAllText(directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First().FullName);
        Save savedData = JsonConvert.DeserializeObject<Save>(savedDataText);
        NestedStatus nestedStatus = savedData.status;

        news.text = "";
        news.text = "Vehicle: " + newStatus.vehicle[nestedStatus.vehicle] + "\n" +
                    "Health: " + newStatus.health[nestedStatus.health] + "\n" +
                    "Social Status: " + newStatus.socialStatus[nestedStatus.socialStatus] + "\n" +
                    "Living: " + newStatus.living[nestedStatus.living];
    }

    public void LoadMainScene(){
        SceneManager.LoadScene("SampleScene");
    }
}