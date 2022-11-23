using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WaveClicked : MonoBehaviour
{
    private GameObject radioScreen;
    private TMP_Text gameText;
    private RadioConfig radio1;
    private RadioConfig radio2;
    private RadioConfig radio3;

    // Start is called before the first frame update
    void Start()
    {
        radioScreen = GameObject.Find("radioScreen");
        gameText = GameObject.Find("Subtitles").GetComponent<TextMeshProUGUI>();

        radio1 = new RadioConfig(1, new Color(1,0,0), 0.0f, "Hello hello hello");
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Time.time > nextActionTime ) {
            nextActionTime += period;
            // execute block of code here
            subtitles.text = "Time je: " + nextActionTime;
        }
        */
        
    }

    public void StartVoice(){
        Debug.Log("CLickol si na wave button :)");
        gameText.enabled = !gameText.enabled;
    }

    public void LoadMinigame1(){
        Debug.Log("sceneName to load: " + "/minigames/minigame-1");
        SceneManager.LoadScene("minigame-1");
    }

public void changeRadioMessage(string newText, int radioNumber){
  if(radioNumber == 1){
    gameText.text = Time.time+".";
  }
  if(radioNumber == 2){

  }
  if(radioNumber == 3){

  }
}

}
