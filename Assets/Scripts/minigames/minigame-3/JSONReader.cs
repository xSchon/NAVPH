using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    public TextAsset jsonFile;
    private TextEntered script;

    [System.Serializable]
    public class Morse{
        public string morse_text;
        public string ascii_text;
        public float guess_time;
    }

    [System.Serializable]
    public class MorseList{
        public Morse[] morse;
    }

    public MorseList morse_array = new MorseList();
    // Start is called before the first frame update
    void Start()
    {
        script = FindObjectOfType<TextEntered>();
        morse_array = JsonUtility.FromJson<MorseList>(jsonFile.text);

        Morse selected_morse = morse_array.morse[Random.Range(0, morse_array.morse.Length)];
        
        Debug.Log(morse_array.morse.Length);
        Debug.Log(selected_morse.morse_text + selected_morse.ascii_text + selected_morse.guess_time);

        script.SetAttributes(selected_morse.morse_text, selected_morse.ascii_text, selected_morse.guess_time);
    }
}
