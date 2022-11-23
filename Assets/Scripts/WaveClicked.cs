using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WaveClicked : MonoBehaviour
{
    private GameObject radioScreen;
    private TMP_Text gameText;
     private RadioConfig[] radios;
    // Start is called before the first frame update
    void Start()
    {
        radioScreen = GameObject.Find("radioScreen");
        gameText = GameObject.Find("Subtitles").GetComponent<TextMeshProUGUI>();
        radios = 
        new RadioConfig[3]
        {new RadioConfig(1, new Color (1, 0, 1), 0.0f, "afk"), 
        new RadioConfig(2, new Color (1, 0, 1), 0.0f, "xdd"), 
        new RadioConfig(3, new Color (1, 0, 1), 0.0f, "red")};

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartVoice(){
        Debug.Log("CLickol si na wave button :)");
        gameText.enabled = !gameText.enabled;
    }

    public void LoadMinigame1(){
        Debug.Log("sceneName to load: " + "/minigames/minigame-1");
        SceneManager.LoadScene("minigame-1");
    }

    public void loadScene(int radioNumber){
        Debug.Log("Selected radio" + radioNumber);
        Debug.Log(radios.Length);
        gameText.text = radios[radioNumber].getMessage();
    }

}