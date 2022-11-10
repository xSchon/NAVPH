using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static radioBehavior;
using TMPro;

public class WaveClicked : MonoBehaviour
{
    private GameObject radioScreen;
    private TMP_Text subtitles;
    private float nextActionTime = 0.0f;
    public float period = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        radioScreen = GameObject.Find("SpeachToText");
        subtitles = GameObject.Find("Subtitles").GetComponent<TextMeshProUGUI>();
        //radioScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime ) {
            nextActionTime += period;
            // execute block of code here
            subtitles.text = "Time je: " + nextActionTime;
        }
        
    }

    public void StartVoice(){
        Debug.Log("CLickol si na wave button :)");
        //subtitles.text = "text";
        subtitles.enabled = !subtitles.enabled;
    }

    public void CloseCanvas(){
        radioScreen.SetActive(false);
    }
}
