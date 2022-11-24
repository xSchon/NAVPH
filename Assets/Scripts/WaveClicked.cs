using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class WaveClicked : MonoBehaviour
{
    private GameObject radioScreen;
    private TMP_Text gameText;
     private RadioConfig[] radios;
     private int activeRadio = 0;
    // Start is called before the first frame update
    void Start()
    {
        radioScreen = GameObject.Find("radioScreen");
        gameText = GameObject.Find("Subtitles").GetComponent<TextMeshProUGUI>();
        radios = 
        new RadioConfig[3]
        {new RadioConfig(1, new Color (0.2f, 0.6f, 0.55f)), 
        new RadioConfig(2, new Color (0.23f, 0.15f, 0.12f)), 
        new RadioConfig(3, new Color (0.3f, 0.3f, 0.27f))};
    }   

    // Update is called once per frame
    void Update()
    {
        updatePosX();
    }

    private void updatePosX(){
        this.radios[activeRadio].setPosX(GameObject.Find("FindingCursor").GetComponent<RectTransform>().localPosition.x);
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
        GameObject.Find("radioBackground").GetComponent<Image>().color = radios[radioNumber].getColor();
        gameText.text = radios[radioNumber].getMessage();
        
        this.activeRadio = radioNumber;
        RectTransform tmp = GameObject.Find("FindingCursor").GetComponent<RectTransform>();
        tmp.localPosition = new Vector3(this.radios[activeRadio].getPosX(), tmp.localPosition.y, tmp.localPosition.z);

        GameObject.Find("FindingCursor").GetComponent<SearchMessage>().setSearch(radios[radioNumber].isActive());
    }

}