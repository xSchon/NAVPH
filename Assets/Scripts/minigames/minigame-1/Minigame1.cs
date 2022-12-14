using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Minigame1 : MonoBehaviour
{
    public GameObject brick;
    private GameObject active_brick;
    private List<GameObject> placedBricks = new List<GameObject>();
    private float[] lastScore = new float[10]; 
    public float endScore = 7.0f;
    public string lostText;
    public string winText;
    private float score = 0f;
    public float number_of_bricks = 10f;

    private TMP_Text ui_score;
    private TMP_Text ui_bricks;
    private bool end_game = false;
    private TMP_Text popup;

    private GameObject button;
    private Camera minigame_camera;
    // Start is called before the first frame update
    void Start()
    {
        SpawNewBrick();
        ui_score = GameObject.Find("score").GetComponent<TextMeshProUGUI>();
        ui_bricks = GameObject.Find("nm_bricks").GetComponent<TextMeshProUGUI>();
        popup = GameObject.Find("popup").GetComponent<TextMeshProUGUI>();
        button = GameObject.Find("button");
        minigame_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        popup.enabled = false;
        button.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)){
            SpawNewBrick();
        }
        UpdateUI();

        IncrementScore();
        
    }

    public void SpawNewBrick(){
        CheckScore();
        if (!end_game) {
            active_brick = Instantiate(brick,
                        transform.position,
                        Quaternion.identity);
        
            number_of_bricks--;
        }
    }

    public void IncrementScore(){
        float newScore = 0f;

        foreach(GameObject brickHeight in placedBricks)
        {
            if (brickHeight.transform.position.y > newScore) 
                newScore = brickHeight.transform.position.y;
        }

        score = newScore;
    }

    public void AddPlacedBrick(GameObject brick)
    {
        placedBricks.Add(brick);
    }

    private void CheckScore(){
        if (score > endScore){
            popup.enabled = true;
            button.SetActive(true);
            popup.text = winText;
            end_game = true;
            PlayerPrefs.SetInt("WonMinigame", 1);
        }

        if(number_of_bricks <= 0){
            number_of_bricks--;
            popup.enabled = true;
            button.SetActive(true);
            popup.text = lostText;
            end_game = true;
            PlayerPrefs.SetInt("WonMinigame", 0);
        }
    }

    void UpdateUI(){
        ui_score.text = "Height: " + score.ToString("F2");
        ui_bricks.text = "Bricks: " + (number_of_bricks + 1);
   }

   public void BackToMenu(){
        int countLoaded = SceneManager.sceneCount;
        int parent_scene_id = -1;
        for (int i = 0; i < countLoaded; i++)
        {
            
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != "minigame-1"){
                parent_scene_id = i;
            }
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneAt(parent_scene_id));
        minigame_camera.enabled = false;
   }
}
