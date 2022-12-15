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
    public GameObject radioScreen;
    private GameObject readingScreen;
    private GameObject[] clickable;
    private TMP_Text gameText;
    private RadioConfig[] radios;
    private SectorsDefence sectrsDeff;
    public GameObject susBarObject;
    private SusBar susBar;
    private int activeRadio = 0;
    public float minigameChance = 0.2f;
    private int[] minigamesIDs;
    private bool loadAfterMinigame = false;
    private Scene main_scene;
    public Camera mainCamera;
    private Camera minigame_camera;
    private EventSystem mainEventSystem;
    private Dictionary<int, List<int>> resetSearch  = new Dictionary<int, List<int>>();
    private Timer timer;
    [SerializeField] private AudioSource radioStatic;

    
    void Start()
    {
        readingScreen = GameObject.Find("ReadingScreen");
        readingScreen.SetActive(false);
        sectrsDeff = GameObject.Find("selectedSectors").GetComponent<SectorsDefence>();
        susBar = susBarObject.GetComponent<SusBar>();

        gameText = GameObject.Find("Subtitles").GetComponent<TextMeshProUGUI>();
        timer = GameObject.Find("DailyTimer").GetComponent<Timer>();
        radios = 
        new RadioConfig[3]
        {new RadioConfig(1, new Color (0.2f, 0.6f, 0.55f)), 
        new RadioConfig(2, new Color (0.23f, 0.15f, 0.12f)), 
        new RadioConfig(3, new Color (0.3f, 0.3f, 0.27f))};

        main_scene = SceneManager.GetActiveScene();
        //mainCamera = (Camera) FindObjectOfType(typeof(Camera));
        mainEventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        SceneManager.activeSceneChanged += returnScene;
        clickable = GameObject.FindGameObjectsWithTag("Clickable");
    }   

    void Update()
    {
        updatePosX();
    }

    private void updatePosX(){
        this.radios[activeRadio].setPosX(GameObject.Find("FindingCursor").GetComponent<RectTransform>().localPosition.x);
    }

    IEnumerator LoadMinigame(int index){
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

        mainCamera.enabled = false;
        radioScreen.SetActive(false);
        mainEventSystem.enabled = false;

        foreach(GameObject clickObject in clickable){
            clickObject.SetActive(false);
        }
    }

    public void loadScene(int radioNumber)
    {
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

    private void returnScene(Scene arg0, Scene arg1)
    {
        if (arg1.name == main_scene.name && arg0.name != "Summary")
        {
            AsyncOperation async = SceneManager.UnloadSceneAsync(arg0.name);
            async.completed += OpenRadioScreen;
        }
    }

    private void OpenRadioScreen(AsyncOperation async)
    {
        mainCamera.enabled = true;
        mainEventSystem.enabled = true;

        foreach(GameObject clickObject in clickable){
            clickObject.SetActive(true);
        }

        loadAfterMinigame = false;

        Debug.Log("Pred: " + susBar.GetSusValue());
        if (Convert.ToBoolean(PlayerPrefs.GetInt("WonMinigame")))
        {
            susBar.DecreaseSus();
        } 
        else 
        {
            susBar.IncreaseSus();
        }
        Debug.Log("Po: " + susBar.GetSusValue());
        radioScreen.SetActive(true);
    }

    public void setMinigames(int[] minigamesIndexes)
    {
        minigamesIDs = minigamesIndexes;
    }

    public void radioActivation(Conversation activeConvo)
    {
        int radioNumber = activeConvo.radio - 1;
        radios[radioNumber].setActive(false);
        try{
            if (this.activeRadio == radioNumber){
                loadScene(radioNumber + 1);
            }}
        catch (NullReferenceException){
            // Expected exception for the time when radio is closed
        };

        int renewSearch = timer.mmHHtoMinutes(activeConvo.hourTill);
        if (this.resetSearch.ContainsKey(renewSearch)){
            List<int> tmp = this.resetSearch[renewSearch];
            tmp.Add(radioNumber);
            this.resetSearch[renewSearch] = tmp;
        } else {
            this.resetSearch.Add(renewSearch, (new List<int>{radioNumber}));
        }

        // Add new information to map, information obtained from radio text
        DefendableSector deff = new DefendableSector();
        deff.sectorNum = activeConvo.sector;
        deff.storyNum = activeConvo.storyLine;
        deff.susPunish = activeConvo.susMeterPenalisation;
        sectrsDeff.NewToDeffend(
            timer.mmHHtoMinutes(activeConvo.whenDeffendSector), deff
        );

        radios[radioNumber].setRadioArray(activeConvo.text);     
        radios[radioNumber].setAuthor(activeConvo.author);
    }

    public void checkStopped(int currentTime)
    {
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

    public void StartVoice()
    {
        CheckMinigame();
        if (!loadAfterMinigame)
            ClickedRead();
    }
    public void ClickedRead()
    {
        // pause radioStatic sound
        radioStatic.Pause();
        string[] showText = radios[this.activeRadio].getRadioArray();
        string authr = radios[this.activeRadio].getAuthor();

        readingScreen.SetActive(true);
        timer.StopTimer();

        // read message 
        readingScreen.GetComponent<ReadingStory>().DisplayConversation(authr, showText, gameText);
    }

    public void FinishedRead()
    {
        radioStatic.Play();
        timer.StartTimer();
        readingScreen.SetActive(false);
        radios[activeRadio].setActive(true);
        loadScene(activeRadio + 1);
    }

    private void CheckMinigame()
    {
        float probability = UnityEngine.Random.Range(0.0f, 1.0f);

        if (probability <= minigameChance){
            loadAfterMinigame = true;
            minigameChance = minigameChance / 2;
            timer.StopTimer();

            int index = UnityEngine.Random.Range(0, minigamesIDs.Length);
            //int index = 1;          // remove!!!!!!!!!!
            StartCoroutine(LoadMinigame(minigamesIDs[index]));
        }
        else {
            minigameChance += 0.1f;
        }
    }
}