/* Class responsible for loading morse from JSON, picking one and sending to TextEntered script */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    public TextAsset jsonFile;
    public TextEntered script;
    private Morse selectedMorse;
    private MorseList morseArray = new MorseList();

    void Start()
    {
        morseArray = JsonConvert.DeserializeObject<MorseList>(jsonFile.text);
        selectedMorse = morseArray.morse[Random.Range(0, morseArray.morse.Length)];
        script.SetAttributes(selectedMorse.morseText, selectedMorse.asciiText, selectedMorse.guessTime);
    }
}
