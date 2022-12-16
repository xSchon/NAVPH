/* Main script for minigame-2 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Main : MonoBehaviour
{

    static public Main Instance;

    private int count = 0;
    private int ending = 9;
    private float timeRemaining;
    private float timeRemainingSeconds;
    [SerializeField] private TMPro.TextMeshProUGUI timerTextMesh;
    [SerializeField] private Canvas popUpCanvas;
    [SerializeField] private TMPro.TextMeshProUGUI popUpText;
    private string winText = "Good job and quick action!";
    private string loseText = "What the hell are you doing, Comrade? You can't stay disconnected from the radio for too long!";
    private bool gameEnd = false;

    private void Awake()
    {
        popUpCanvas.enabled = false;
        Instance = this;
        Time.timeScale = 0.0f;

        int difficulty = PlayerPrefs.GetInt("difficulty", 2);
        if (difficulty == 1) { timeRemaining = 25f; } // easy
        if (difficulty == 2) { timeRemaining = 20f; } // normal
        if (difficulty == 3) { timeRemaining = 15f; } // hard
    }

    void Update()
    {
        if (count == ending)
        {
            timerTextMesh.text = timeRemainingSeconds.ToString();
            gameEnd = true;
            popUpCanvas.enabled = true;
            popUpText.text = winText;
            PlayerPrefs.SetInt("WonMinigame", 1);
        }

        if (timeRemaining < 0 && gameEnd == false)
        {
            PlayerPrefs.SetInt("WonMinigame", 0);
            gameEnd = true;
            popUpCanvas.enabled = true;
            popUpText.text = loseText;

        }
        if (gameEnd == false && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            // remove decimal places from timeRemaining
            timeRemainingSeconds = Mathf.Round(timeRemaining * 100f) / 100f;
            timerTextMesh.text = timeRemainingSeconds.ToString();
        }
    }

    public void AddScore()
    {
        count++;
    }

    public void UnfreezeTime()
    {
        Time.timeScale = 1.0f;
    }
}
