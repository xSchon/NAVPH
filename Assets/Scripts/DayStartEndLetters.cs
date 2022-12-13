using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine;
using TMPro;


public class DayStartEndLetters : MonoBehaviour
{
    public TextAsset jsonFile;
    public TextAsset saveJsonFile;
    private Save savedData;
    private Dictionary <string, Dictionary<string, Dictionary<string, string>>> dailyMessages;
    public string dayIndex;
    private string letterText;
    [SerializeField] private TMPro.TextMeshProUGUI letterTextMesh;

    void Start()
    {
        var directory = new DirectoryInfo(Application.persistentDataPath);
        var files = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).Where(f => f.Name != "prefs");
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
            dayIndexInt++;
            dayIndex = dayIndexInt.ToString();
            
            Debug.Log(dayIndex);
        // testovacie, nebude to tu hardcoded 
            if (dayIndex == "5")
            {
                Debug.Log("Ending");
                //SceneManager.LoadScene("Ending");
                return;
            }
            else 
            {
//
            }
            //Debug.Log(dayIndex);
        }

        dailyMessages = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(jsonFile.text);
        letterText = dailyMessages[dayIndex]["Morning"]["Text1"];
        letterTextMesh.text = letterText;
    }

    // public void EndDayLetter() // TO DO
    // { 
    //     // public void endDay() vola zmenu sceny na Summary 
    //     // V scene summary - zmena textu 
    //     dailyMessages = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(jsonFile.text);
    //     Debug.Log(dailyMessages["1"]["Evening"]["Text1"]);
    //     newspaperText1 = dailyMessages[dayIndex]["Evening"]["Text1"];
    //     newspaperText1Mesh.text = newspaperText1;
    // }

}
