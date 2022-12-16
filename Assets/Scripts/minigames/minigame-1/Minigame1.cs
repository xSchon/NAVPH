using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Minigame1 : MonoBehaviour
{
    public GameObject brick;
    private GameObject activeBrick;
    private List<GameObject> placedBricks = new List<GameObject>();
    public float endScore = 6.0f;
    public string lostText;
    public string winText;
    private float score = 0f;
    public float numberOfBricks = 10f;
    public TMP_Text UIscore;
    public TMP_Text UIbricks;
    private bool endGame = false;
    public TMP_Text popup;
    public GameObject button;
    public Camera minigameCamera;
    public Canvas tutorial;
    
    void Start()
    {
        popup.enabled = false;
        button.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)){
            SpawNewBrick();
        }
        UpdateUI();

        IncrementScore();
        
    }

    public void SpawNewBrick(){
        IncrementScore();
        CheckScore();
        if (!endGame) {
            activeBrick = Instantiate(brick,
                        transform.position,
                        Quaternion.identity);
        
            numberOfBricks--;
        }
    }

    public void IncrementScore(){
        float newScore = 0f;

        foreach(GameObject brickHeight in placedBricks)
        {
            if (brickHeight.transform.position.y > newScore){
                newScore = brickHeight.transform.position.y;
            }
        }

        score = newScore;
    }

    private void CheckScore(){
        if (score > endScore){
            popup.enabled = true;
            button.SetActive(true);
            popup.text = winText;
            endGame = true;
            PlayerPrefs.SetInt("WonMinigame", 1);
            Time.timeScale = 0.0f;
        }

        if(numberOfBricks <= 0){
            numberOfBricks--;
            popup.enabled = true;
            button.SetActive(true);
            popup.text = lostText;
            endGame = true;
            PlayerPrefs.SetInt("WonMinigame", 0);
        }
    }

    public void AddPlacedBrick(GameObject brick)
    {
        placedBricks.Add(brick);
    }

    void UpdateUI(){
        UIscore.text = "Height: " + score.ToString("F2");
        UIbricks.text = "Bricks: " + (numberOfBricks + 1);
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
        minigameCamera.enabled = false;
    }

   public void EndTutorial()
   {
        tutorial.enabled = false;
   }
}
