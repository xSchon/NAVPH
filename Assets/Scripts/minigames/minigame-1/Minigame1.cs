using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Minigame1 : MonoBehaviour
{
    private float exec_time = 0.0f;
    public float period = 5.0f;
    private float score = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            endScene();
        }
    }

    void endScene(){
        Debug.Log("Vypinam scenu novu :(");
        SceneManager.LoadScene("SampleScene");
    }

    public void incremenmtScore(float height){
        if (height > score) score = height;
        Debug.Log(score);
    }


}
