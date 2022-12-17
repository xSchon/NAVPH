/* Game pase after user's click on the button */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseScreen;

    void Start()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 0.0f;
    }

    public void StartPause()
    {
        Debug.Log("Pause started");
        Time.timeScale = 0.0f;
        pauseScreen.SetActive(true);
    }

    public void EndPause()
    {
        Debug.Log("Pause ended, game continues");
        Time.timeScale = 1.0f;
        pauseScreen.SetActive(false);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
