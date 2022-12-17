/* Script taking care of radio and its messages */
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class RadioLogic : MonoBehaviour
{
    public GameObject radioScreen;
    public GameObject readingScreen;
    public GameObject findingCursor;
    private GameObject[] clickable;
    public TMP_Text gameText;
    private RadioConfig[] radios;
    public SectorsDefence sectrsDeff;
    public SusBar susBar;
    public Button waveButton;
    private int activeRadio = 0;
    private float minigameChance = 0.2f;
    private int[] minigamesIDs;
    private bool loadAfterMinigame = false;
    private Scene mainScene;
    public Camera mainCamera;
    private Camera minigameCamera;
    public EventSystem mainEventSystem;
    private Dictionary<int, List<int>> resetSearch = new Dictionary<int, List<int>>();
    public Timer timer;
    public AudioSource radioStatic;
    public RectTransform cursorPos;
    public Image radioBackground;


    void Start()
    {
        readingScreen.SetActive(false);

        radios =
        new RadioConfig[3]
        {new RadioConfig(1, new Color (0.2f, 0.6f, 0.55f)),
        new RadioConfig(2, new Color (0.23f, 0.15f, 0.12f)),
        new RadioConfig(3, new Color (0.3f, 0.3f, 0.27f))};

        mainScene = SceneManager.GetActiveScene();
        SceneManager.activeSceneChanged += ReturnScene;
    }

    void Update()
    {
        this.radios[activeRadio].SetPosX(cursorPos.localPosition.x);
    }

    // Method that additively loads minigame, waits till it is loaded.
    private IEnumerator LoadMinigame(int index)
    {
        mainEventSystem.enabled = false;                // Disable one of event systems (from main scene).

        AsyncOperation async = SceneManager.LoadSceneAsync($"minigame-{index}", LoadSceneMode.Additive);    // Load chosen minigame

        while (!async.isDone)                           // Load scene, wait until it is done
        {
            yield return async;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName($"minigame-{index}"));      // Set new scene as main.

        minigameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();             // load new camera from minigame
        mainCamera.enabled = false;                                                         // disable camera in main scene
        radioScreen.SetActive(false);                                                       // disable opened radio screen

        clickable = GameObject.FindGameObjectsWithTag("Clickable");                         // load all objects that should be disabled
        foreach (GameObject clickObject in clickable)
        {
            clickObject.SetActive(false);
        }
    }

    // Load radio and configure it depending on which one was clicked
    public void LoadRadioScene(int radioNumber)
    {
        radioNumber = radioNumber - 1;

        radioBackground.color = radios[radioNumber].GetColor();
        gameText.text = "";
        this.activeRadio = radioNumber;
        RectTransform tmp = findingCursor.GetComponent<RectTransform>();
        tmp.localPosition = new Vector3(this.radios[activeRadio].GetPosX(), tmp.localPosition.y, tmp.localPosition.z);

        findingCursor.GetComponent<SearchMessage>().SetSearch(radios[radioNumber].IsActive());
        if (!radios[radioNumber].IsActive())
        {
            gameText.text = "...";
            waveButton.enabled = true;
        }
        else
        {
            waveButton.enabled = false;
        }
    }

    // Method which is called after main scene is changed.
    private void ReturnScene(Scene arg0, Scene arg1)
    {
        // If ending scene is minigame and game is returning to Office, unload the minigame scene
        if (arg1.name == mainScene.name && arg0.name != "Summary")
        {
            AsyncOperation async = SceneManager.UnloadSceneAsync(arg0.name);
            async.completed += OpenRadioScreen;
        }
    }

    // Method called after minigame is unloaded.
    private void OpenRadioScreen(AsyncOperation async)
    {
        // Enable camera and mainEventSystem in main scene
        mainCamera.enabled = true;
        mainEventSystem.enabled = true;

        // Enable back disabled objects
        foreach (GameObject clickObject in clickable)
        {
            clickObject.SetActive(true);
        }

        loadAfterMinigame = false;

        // If player won game.
        if (Convert.ToBoolean(PlayerPrefs.GetInt("WonMinigame")))
        {
            susBar.DecreaseSus();
        }
        else
        {
            susBar.IncreaseSus();
        }

        // Enable time in main scene again.
        Time.timeScale = 1.0f;
        // Enable opened radio screen again.
        radioScreen.SetActive(true);
    }

    // Get minigames from json file.
    public void SetMinigames(int[] minigamesIndexes)
    {
        minigamesIDs = minigamesIndexes;
    }

    // Load messages to radio and information about them
    public void RadioActivation(Conversation activeConvo)
    {
        int radioNumber = activeConvo.radio - 1;
        radios[radioNumber].SetActive(false);
        try
        {
            if (this.activeRadio == radioNumber)
            {
                LoadRadioScene(radioNumber + 1);
            }
        }
        catch (NullReferenceException)
        {
            // Expected exception for the time when radio is closed
        };

        int renewSearch = timer.HHMMtoMinutes(activeConvo.hourTill);
        if (this.resetSearch.ContainsKey(renewSearch))
        {
            List<int> tmp = this.resetSearch[renewSearch];
            tmp.Add(radioNumber);
            this.resetSearch[renewSearch] = tmp;
        }
        else
        {
            this.resetSearch.Add(renewSearch, (new List<int> { radioNumber }));
        }

        // Add new information to map, information obtained from radio text
        DefendableSector deff = new DefendableSector();
        deff.sectorNum = activeConvo.sector;
        deff.storyNum = activeConvo.storyLine;
        deff.susPunish = activeConvo.susMeterPenalisation;
        sectrsDeff.NewToDefend(
            timer.HHMMtoMinutes(activeConvo.whenDeffendSector), deff
        );

        radios[radioNumber].SetRadioArray(activeConvo.text);
        radios[radioNumber].SetAuthor(activeConvo.author);
    }

    // Check if the radio should be stopped, load receivec message
    public void CheckStopped(int currentTime)
    {
        if (this.resetSearch.ContainsKey(currentTime))
        {
            foreach (int timeRen in this.resetSearch[currentTime])
            {
                this.radios[timeRen].SetActive(true);
                this.radios[timeRen].SetRadioArray(Array.Empty<string>());
                this.radios[timeRen].SetAuthor("");
                try
                {
                    if (this.activeRadio == timeRen)
                    {
                        LoadRadioScene(timeRen + 1);
                    }
                }
                catch (NullReferenceException)
                {
                    // Expected exception for the time when radio is closed
                };
            }
        }
    }

    // Check if Minigame should pop.
    private void CheckMinigame()
    {
        // Calculate new probability
        float probability = UnityEngine.Random.Range(0.0f, 1.0f);

        // If minigames were not specified
        if (minigamesIDs == null)
            return;

        // If minigame pops
        if (probability <= minigameChance)
        {
            loadAfterMinigame = true;
            minigameChance = minigameChance / 2;                // Divide chance of new minigame
            timer.StopTimer();                                  // Stop timer in main scene

            int index = UnityEngine.Random.Range(0, minigamesIDs.Length);   // Load random minigame from specified 
            StartCoroutine(LoadMinigame(minigamesIDs[index]));              // Load new minigame
        }
        else
        {
            int difficulty = PlayerPrefs.GetInt("difficulty", 2);
            if (difficulty == 1) { minigameChance += 0.05f; } // easy
            if (difficulty == 2) { minigameChance += 0.1f; } // normal
            if (difficulty == 3) { minigameChance += 0.15f; } // hard
        }
    }

    // When clicked on button
    public void StartVoice()
    {
        CheckMinigame();

        // Check if game is not in minigame
        if (!loadAfterMinigame)
            ClickedRead();
    }

    // Reading screen with it's text
    public void ClickedRead()
    {
        // pause radioStatic sound
        radioStatic.Pause();
        string[] showText = radios[this.activeRadio].GetRadioArray();
        string authr = radios[this.activeRadio].GetAuthor();

        readingScreen.SetActive(true);
        timer.StopTimer();

        // read message 
        readingScreen.GetComponent<ReadingStory>().DisplayConversation(authr, showText, gameText);
    }

    // Continue with game after message was received
    public void FinishedRead()
    {
        radioStatic.Play();
        timer.StartTimer();
        readingScreen.SetActive(false);
        radios[activeRadio].SetActive(true);
        LoadRadioScene(activeRadio + 1);
    }

}