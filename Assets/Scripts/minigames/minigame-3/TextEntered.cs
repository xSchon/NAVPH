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
    private bool game_end = false;
    public float timeRemaining;
    public string morse_code;
    public string ascii_code;
    public string lostText = "Message was not deciphered at time, game is lost, your Sus bar will be increased. You have disappointed the socialist people!";
    public string winText = "You got it right, your Sus bar will be decreased! Thank you for your dedicated work!";
    [SerializeField] private AudioSource morseStatic;
    // Start is called before the first frame update
    void Start()
    {
        popupCanvas.enabled = false;
        morseField.text = morse_code;

        outputText.readOnly = true;

        Time.timeScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining < 0){
            popupCanvas.enabled = true;
            popup.text = lostText;
        } else if (!game_end) {
            timeRemaining -= Time.deltaTime;
            timeField.text = timeRemaining.ToString();
        }
    }

    public void TextSubmitted(){
        if (outputText.text.ToLower() == ascii_code){
            popupCanvas.enabled = true;
            popup.text = winText;
            game_end = true;
            morseStatic.Pause();
        }
    }

    public void SetAttributes(string morse_text, string ascii_text, float time){
        this.morse_code = morse_text;
        this.ascii_code = ascii_text;

        int difficulty =  PlayerPrefs.GetInt("difficulty", 2);
        if (difficulty == 1) { this.timeRemaining = time * 1.25f; } // easy
        if (difficulty == 2) { this.timeRemaining = time; } // normal
        if (difficulty == 3) { this.timeRemaining = time * 0.75f; } // hard
    }

    public void UnfreezeTime()
    {
        Time.timeScale = 1.0f;
        outputText.readOnly = false;
    }
}