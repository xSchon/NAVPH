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
        spawNewBrick();
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
            spawNewBrick();
        }
        updateUI();
    }

    public void spawNewBrick(){
        checkScore();
        if (!end_game) {
            active_brick = Instantiate(brick,
                        transform.position,
                        Quaternion.identity);
        
            number_of_bricks--;
        }
    }

    public void incremenmtScore(float height){
        if (height > score) score = height;
    }

    private void checkScore(){
        if (score > 6){
            popup.enabled = true;
            button.SetActive(true);
            popup.text = "Vyhrali ste :) Vas SUS bar sa znizil. Socialisticky lud si vazi vasej pomoci pri obrane statnej hranice! o7";
            Debug.Log("Vyhrali ste :)");
            end_game = true;
        }

        if(number_of_bricks <= 0){
            popup.enabled = true;
            button.SetActive(true);
            popup.text = "Prehrali ste :( Vas SUS bar sa zvysil. Socialisticky lud je sklamany z vasej prace pri obrane statnej hranice!";
            Debug.Log("Prehrali ste :(");
            end_game = true;
        }
    }

    void updateUI(){
        ui_score.text = "Tvoje score je: " + score;
        ui_bricks.text = "Zostavajuci pocet tehal: " + number_of_bricks;
   }

   public void backToMenu(){
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
