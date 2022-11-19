using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void backToMenu(){
        SceneManager.LoadScene("Menu");
       //GameObject.Find("MainMenu").GetComponent<MainMenu>().active = false;
        //GameObject.FindObjectsOfTypeAll("MinigamesMenu").SetActive(true);
   }
}
