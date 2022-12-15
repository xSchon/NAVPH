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
    private float timeRemaining = 15f;
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
    }

    void Update()
    {
        if (count == ending)
        {
            timerTextMesh.text = timeRemainingSeconds.ToString();
            gameEnd = true;
            popUpCanvas.enabled = true;
            popUpText.text = winText;
            Debug.Log("You Win!");
            PlayerPrefs.SetInt("WonMinigame", 1);
        }

        if (timeRemaining < 0 && gameEnd == false)
        {
            Debug.Log("You Lose!");
            PlayerPrefs.SetInt("WonMinigame", 0);
            gameEnd = true;
            popUpCanvas.enabled = true;
            popUpText.text = loseText;

        } 
        if (gameEnd == false && timeRemaining > 0) 
        {
            Debug.Log("Game is still running");
            Debug.Log(gameEnd);
            timeRemaining -= Time.deltaTime;
            // remove decimal places from timeRemaining
            timeRemainingSeconds = Mathf.Round(timeRemaining * 100f) / 100f;
            timerTextMesh.text = timeRemainingSeconds.ToString();
        }   
    }

    public void AddScore()
    {
        count++;
        Debug.Log(count);

    }
}
