using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private GameObject pauseScreen;

    void Start()
    {
        pauseScreen = GameObject.Find("PauseScreen");
        pauseScreen.SetActive(false);
    }

    void Update(){

    }

    public void StartPause(){
        Debug.Log("Pause started");
        Time.timeScale = 0.0f;
        pauseScreen.SetActive(true);
   }

    public void EndPause(){
        Debug.Log("Pause ended, game continues");
        Time.timeScale = 1.0f;    
        pauseScreen.SetActive(false);
    }
}
