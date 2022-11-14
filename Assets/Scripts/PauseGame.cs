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
        Time.timeScale = 0.0f;
        pauseScreen.SetActive(true);
        EndPause();
   }

    public void EndPause(){
        Debug.Log("Hmm");
        Time.timeScale = 1.0f;    
        pauseScreen.SetActive(false);
    }
}
