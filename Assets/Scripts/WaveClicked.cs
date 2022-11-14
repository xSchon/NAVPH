using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WaveClicked : MonoBehaviour
{
    private GameObject radioScreen;
    private TMP_Text subtitles;
    private float nextActionTime = 0.0f;
    public float period = 0.3f;

    private AssetBundle myLoadedAssetBundle;
    private string[] scenePaths;
    // Start is called before the first frame update
    void Start()
    {
        radioScreen = GameObject.Find("radioScreen");
        subtitles = GameObject.Find("Subtitles").GetComponent<TextMeshProUGUI>();
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

    public void LoadMinigame1(){
        Debug.Log("sceneName to load: " + "/minigames/minigame-1");
        SceneManager.LoadScene("minigame-1");
    }
}
