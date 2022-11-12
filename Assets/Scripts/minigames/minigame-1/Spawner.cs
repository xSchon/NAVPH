using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Spawner : MonoBehaviour
{
    public GameObject brick;
    private GameObject active_brick;
    private float score = 0f;
    public float number_of_bricks = 10f;

    private TMP_Text ui_score;
    private TMP_Text ui_bricks;
    // Start is called before the first frame update
    void Start()
    {
        spawNewBrick();
        ui_score = GameObject.Find("score").GetComponent<TextMeshProUGUI>();
        ui_bricks = GameObject.Find("nm_bricks").GetComponent<TextMeshProUGUI>();
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
        active_brick = Instantiate(brick,
                        transform.position,
                        Quaternion.identity);
        
        number_of_bricks--;

        //active_brick.setMe(active_brick);
    }

    public void incremenmtScore(float height){
        if (height > score) score = height;
        Debug.Log(score);
        checkScore();
    }

    private void checkScore(){
        if (score > 6){
            Debug.Log("Vyhrali ste :)");
            SceneManager.LoadScene("SampleScene");
        }

        if(number_of_bricks <= 0){
            Debug.Log("Prehrali ste :(");
            SceneManager.LoadScene("SampleScene");
        }
    }

    void updateUI(){
        ui_score.text = "Tvoje score je: " + score;
        ui_bricks.text = "Zostavajuci pocet tehal: " + number_of_bricks;
   }
}
