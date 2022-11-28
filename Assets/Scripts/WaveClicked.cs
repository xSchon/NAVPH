using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class WaveClicked : MonoBehaviour
{
    private GameObject radioScreen;
    private GameObject readingScreen;
    private GameObject[] clickable;
    private TMP_Text gameText;
    private RadioConfig[] radios;
    private sectorsDeffence sectrsDeff;
    private int activeRadio = 0;
    private int[] minigamesIDs;

    private Scene main_scene;
    private Camera main_camera;
    private Camera minigame_camera;
    private EventSystem mainEventSystem;
    private Dictionary<int, List<int>> resetSearch  = new Dictionary<int, List<int>>();
    private Timer timer;

    
    // Start is called before the first frame update
    void Start()
    {
        radioScreen = GameObject.Find("RadioScreen");
        readingScreen = GameObject.Find("ReadingScreen");
        readingScreen.SetActive(false);
        sectrsDeff = GameObject.Find("selectedSectors").GetComponent<sectorsDeffence>();

        gameText = GameObject.Find("Subtitles").GetComponent<TextMeshProUGUI>();
        timer = GameObject.Find("DailyTimer").GetComponent<Timer>();
        radios = 
        new RadioConfig[3]
        {new RadioConfig(1, new Color (0.2f, 0.6f, 0.55f)), 
        new RadioConfig(2, new Color (0.23f, 0.15f, 0.12f)), 
        new RadioConfig(3, new Color (0.3f, 0.3f, 0.27f))};

        main_scene = SceneManager.GetActiveScene();
        main_camera = (Camera) FindObjectOfType(typeof(Camera));
        mainEventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        SceneManager.activeSceneChanged += returnScene;
        
        
    }   

    // Update is called once per frame
    void Update()
    {
        updatePosX();
    }

    private void updatePosX(){
        this.radios[activeRadio].setPosX(GameObject.Find("FindingCursor").GetComponent<RectTransform>().localPosition.x);
    }

    IEnumerator LoadMinigame1(int index){
        AsyncOperation async = SceneManager.LoadSceneAsync($"minigame-{index}", LoadSceneMode.Additive); 
        
        while (!async.isDone)
        {
            Debug.Log("nacitavam scenu");
            yield return async;
            //yield return new WaitForEndOfFrame();
            //GUIManager.instance.guiLoading.setProgress(async.progress);
        }
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName($"minigame-{index}"));

        minigame_camera = GameObject.Find("Main Camera").GetComponent<Camera>();

        main_camera.enabled = false;
        radioScreen.SetActive(false);
        mainEventSystem.enabled = false;
        
        clickable = GameObject.FindGameObjectsWithTag("Clickable");

        foreach(GameObject clickObject in clickable){
            clickObject.SetActive(false);
        }
    }

    public void loadScene(int radioNumber){
        radioNumber = radioNumber - 1;

        GameObject.Find("radioBackground").GetComponent<Image>().color = radios[radioNumber].getColor();
        gameText.text = "";
        this.activeRadio = radioNumber;
        RectTransform tmp = GameObject.Find("FindingCursor").GetComponent<RectTransform>();
        tmp.localPosition = new Vector3(this.radios[activeRadio].getPosX(), tmp.localPosition.y, tmp.localPosition.z);

        GameObject.Find("FindingCursor").GetComponent<SearchMessage>().setSearch(radios[radioNumber].isActive());
        if(!radios[radioNumber].isActive()){
            gameText.text = "...";
            GameObject.Find("WaveButton").GetComponent<Button>().enabled = true;
        } else {
            GameObject.Find("WaveButton").GetComponent<Button>().enabled = false;
        }
    }

    public void returnScene(Scene arg0, Scene arg1){
        if (arg1.name == main_scene.name)
            SceneManager.UnloadSceneAsync(arg0.name);
            main_camera.enabled = true;
            mainEventSystem.enabled = true;

            foreach(GameObject clickObject in clickable){
                clickObject.SetActive(true);
            }
    }

    public void setMinigames(int[] minigamesIndexes){
        minigamesIDs = minigamesIndexes;
    }

    public void radioActivation(Conversation activeConvo){
        int radioNumber = activeConvo.Radio - 1;
        radios[radioNumber].setActive(false);
        try{
            if (this.activeRadio == radioNumber){
                loadScene(radioNumber + 1);
            }}
        catch (NullReferenceException){
            // Expected exception for the time when radio is closed
        };

        int renewSearch = timer.mmHHtoMinutes(activeConvo.HourTill);
        if (this.resetSearch.ContainsKey(renewSearch)){
            List<int> tmp = this.resetSearch[renewSearch];
            tmp.Add(radioNumber);
            this.resetSearch[renewSearch] = tmp;
        } else {
            this.resetSearch.Add(renewSearch, (new List<int>{radioNumber}));
        }

        // Add new information to map, information obtained from radio text
        DeffendableSector deff = new DeffendableSector();
        deff.sectorNum = activeConvo.Sector;
        deff.storyNum = activeConvo.StoryLine;
        deff.susPunish = activeConvo.SusMeterPenalisation;
        sectrsDeff.NewToDeffend(
            timer.mmHHtoMinutes(activeConvo.WhenDeffendSector), deff
        );

        radios[radioNumber].setRadioArray(activeConvo.Text);     
        radios[radioNumber].setAuthor(activeConvo.Author);
    }

    public void checkStopped(int currentTime){
        if (this.resetSearch.ContainsKey(currentTime)){
            foreach(int timeRen in this.resetSearch[currentTime]){
                this.radios[timeRen].setActive(true);
                this.radios[timeRen].setRadioArray(Array.Empty<string>());
                this.radios[timeRen].setAuthor("");
                try{
                    if (this.activeRadio == timeRen){
                        loadScene(timeRen + 1);
                    }} catch (NullReferenceException){
                        // Expected exception for the time when radio is closed
                    };
            }
        }
    }

    public void StartVoice(){
        float probability = UnityEngine.Random.Range(0.0f, 1.0f);
        Debug.Log(probability);

        if (probability <= 0.0f){
            int index = UnityEngine.Random.Range(0, minigamesIDs.Length);
            StartCoroutine(LoadMinigame1(minigamesIDs[index]));
        }

        ClickedRead();
    }
    public void ClickedRead(){
        string[] showText = radios[this.activeRadio].getRadioArray();
        string authr = radios[this.activeRadio].getAuthor();

        readingScreen.SetActive(true);
        timer.StopTimer();

        // read message 
        readingScreen.GetComponent<ReadingStory>().DisplayConversation(authr, showText, gameText);
    }

    public void FinishedRead(){
        timer.StartTimer();
        readingScreen.SetActive(false);
        radios[activeRadio].setActive(true);
        loadScene(activeRadio + 1);
    }

}