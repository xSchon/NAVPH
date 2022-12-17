/* Manager script for minigame-3 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TextEntered : MonoBehaviour
{
    public TMP_InputField outputText;
    public TMP_Text morseField;
    public TMP_Text timeField;
    public TMP_Text popup;
    public GameObject button;
    public Canvas popupCanvas;
    private bool gameEnd = false;
    public float timeRemaining;
    public string morseCode;
    public string asciiCode;
    public string lostText = "Message was not deciphered at time, game is lost, your Sus bar will be increased. You have disappointed the socialist people!";
    public string winText = "You got it right, your Sus bar will be decreased! Thank you for your dedicated work!";
    [SerializeField] private AudioSource morseStatic;
    
    void Start()
    {
        morseField.enabled = false;
        popupCanvas.enabled = false;
        morseField.text = morseCode;
        outputText.readOnly = true;
        Time.timeScale = 0.0f;
    }

    void Update()
    {
        if (timeRemaining < 0)
        {
            popupCanvas.enabled = true;
            popup.text = lostText;
            PlayerPrefs.SetInt("WonMinigame", 0);
        }
        else if (!gameEnd)
        {
            timeRemaining -= Time.deltaTime;
            timeField.text = timeRemaining.ToString("F2");;
            PlayerPrefs.SetInt("WonMinigame", 1);
        }
    }

    // Check submitted text
    public void TextSubmitted()
    {   
        // If correct, end game
        if (outputText.text.ToLower() == asciiCode)
        {
            popupCanvas.enabled = true;
            popup.text = winText;
            gameEnd = true;
            morseStatic.Pause();
        }
    }

    // Set codes and time by difficulty
    public void SetAttributes(string morseText, string asciiText, float time)
    {
        this.morseCode = morseText;
        this.asciiCode = asciiText;

        int difficulty = PlayerPrefs.GetInt("difficulty", 2);
        if (difficulty == 1) { this.timeRemaining = time * 1.25f; } // easy
        if (difficulty == 2) { this.timeRemaining = time; } // normal
        if (difficulty == 3) { this.timeRemaining = time * 0.6f; } // hard
    }

    // Freeze game after correct guess
    public void UnfreezeTime()
    {
        Time.timeScale = 1.0f;
        outputText.readOnly = false;
        morseField.enabled = true;
    }
}