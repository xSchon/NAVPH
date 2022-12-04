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
    private Dictionary <string, Dictionary<string, Dictionary<string, string>>> dailyMessages;
    private string currentDay = "1"; // tu bude nacitavanie current dna z json save
    private string letterText;
    [SerializeField] private TMPro.TextMeshProUGUI letterTextMesh;
    void Start()
    {
        dailyMessages = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(jsonFile.text);
        letterText = dailyMessages[currentDay]["Morning"]["Text1"];
        letterTextMesh.text = letterText;

    }

}
