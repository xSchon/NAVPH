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

    private Scene main_scene;
    Camera main_camera;
    Camera minigame_camera;
    EventSystem mainEventSystem;
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
        StartCoroutine(LoadMinigame1());
    }

    IEnumerator LoadMinigame1(){
        Debug.Log("Aktivna scena je: " +  SceneManager.GetActiveScene().name);
        Debug.Log("sceneName to load: " + "/minigames/minigame-1");

        AsyncOperation async = SceneManager.LoadSceneAsync("minigame-1", LoadSceneMode.Additive); 

        //yield return async;
        

        Debug.Log("Aktivna scena je: " +  SceneManager.GetActiveScene().name);
        
        while (!async.isDone)
        {
            Debug.Log("nacitavam scenu");
            yield return async;
            //yield return new WaitForEndOfFrame();
            //GUIManager.instance.guiLoading.setProgress(async.progress);
        }

        Debug.Log("Aktivna scena je: " +  SceneManager.GetActiveScene().name);
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("minigame-1"));

        Debug.Log("Aktivna scena je: " +  SceneManager.GetActiveScene().name);

        minigame_camera = GameObject.Find("Main Camera").GetComponent<Camera>();

        main_camera.enabled = false;
        radioScreen.SetActive(false);
        mainEventSystem.enabled = false;
        //minigame_camera.SetActive(true);
        //SceneHelper.LoadScene( "minigame-1", additive: true, setActive: true);
    }

    public void loadScene(int radioNumber){
        GameObject.Find("radioBackground").GetComponent<Image>().color = radios[radioNumber].getColor();
        gameText.text = radios[radioNumber].getMessage();
        
        this.activeRadio = radioNumber;
        RectTransform tmp = GameObject.Find("FindingCursor").GetComponent<RectTransform>();
        tmp.localPosition = new Vector3(this.radios[activeRadio].getPosX(), tmp.localPosition.y, tmp.localPosition.z);

        GameObject.Find("FindingCursor").GetComponent<SearchMessage>().setSearch(radios[radioNumber].isActive());
    }

    public void returnScene(Scene arg0, Scene arg1){
        if (arg1.name == main_scene.name)
            SceneManager.UnloadSceneAsync(arg0.name);
            main_camera.enabled = true;
            mainEventSystem.enabled = true;
    }
}