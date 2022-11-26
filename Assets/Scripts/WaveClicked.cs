using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class WaveClicked : MonoBehaviour
{
    private GameObject radioScreen;
    private TMP_Text gameText;
    private RadioConfig[] radios;
    private int activeRadio = 0;
    private int[] minigamesIDs;

    private Scene main_scene;
    private Camera main_camera;
    private Camera minigame_camera;
    private EventSystem mainEventSystem;

    private GameObject[] clickable;
    // Start is called before the first frame update
    void Start()
    {
        radioScreen = GameObject.Find("RadioScreen");
        gameText = GameObject.Find("Subtitles").GetComponent<TextMeshProUGUI>();
        radios = 
        new RadioConfig[3]
        {new RadioConfig(1, new Color (0.2f, 0.6f, 0.55f)), 
        new RadioConfig(2, new Color (0.23f, 0.15f, 0.12f)), 
        new RadioConfig(3, new Color (0.3f, 0.3f, 0.27f))};

        main_scene = SceneManager.GetActiveScene();
        main_camera = (Camera) FindObjectOfType(typeof(Camera));
        mainEventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        SceneManager.activeSceneChanged += returnScene;

        clickable = GameObject.FindGameObjectsWithTag("Clickable");
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

        float probability = Random.Range(0.0f, 1.0f);
        Debug.Log(probability);
        if (probability <= 0.3f){
            int index = Random.Range(0, minigamesIDs.Length);
            StartCoroutine(LoadMinigame1(minigamesIDs[index]));
        }
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

        foreach(GameObject clickObject in clickable){
            clickObject.SetActive(false);
        }
    }

    public void loadScene(int radioNumber){
        GameObject.Find("radioBackground").GetComponent<Image>().color = radios[radioNumber].getColor();
        gameText.text = radios[radioNumber].getMessage();
        
        this.activeRadio = radioNumber;
        RectTransform tmp = GameObject.Find("FindingCursor").GetComponent<RectTransform>();
        tmp.localPosition = new Vector3(this.radios[activeRadio].getPosX(), tmp.localPosition.y, tmp.localPosition.z);

        GameObject.Find("FindingCursor").GetComponent<SearchMessage>().setSearch(radios[radioNumber].isActive());
    }

    private void returnScene(Scene arg0, Scene arg1){
        if (arg1.name == main_scene.name)
            SceneManager.UnloadSceneAsync(arg0.name);
            main_camera.enabled = true;
            mainEventSystem.enabled = true;

            Debug.Log(clickable.Length);

            foreach(GameObject clickObject in clickable){
                Debug.Log(clickObject);
                clickObject.SetActive(true);
            }
    }

    public void setMinigames(int[] minigamesIndexes){
        minigamesIDs = minigamesIndexes;
    }
}