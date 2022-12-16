using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI soundTextMesh;
    [SerializeField] private TMPro.TextMeshProUGUI difficultyTextMesh;

    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetInt("volume", 1);
        if (AudioListener.volume == 0)
        {
            soundTextMesh.text = "OFF";
        }
        else
        {
            soundTextMesh.text = "ON";
        }

        int currentDiff =  PlayerPrefs.GetInt("difficulty", 2);
        if (currentDiff == 1) { difficultyTextMesh.text = "Easy"; } // easy
        if (currentDiff == 2) { difficultyTextMesh.text = "Medium"; } // normal
        if (currentDiff == 3) { difficultyTextMesh.text = "Hard"; } // hard
    }

    public void MuteAllSounds()
    {
        if (soundTextMesh.text == "ON")
        {
            AudioListener.volume = 0;
            soundTextMesh.text = "OFF";
            Debug.Log("Mute");
        }
        else
        {
            AudioListener.volume = 1;
            soundTextMesh.text = "ON";
            Debug.Log("Unmute");
        }
        PlayerPrefs.SetInt("volume", AudioListener.volume);
        PlayerPrefs.Save();
    }

    public void SetDifficulty()
    {
        int difficulty = 2;
        if (difficultyTextMesh.text == "Easy")
        {
            difficulty = 2;
            difficultyTextMesh.text = "Medium";
        }
        else if (difficultyTextMesh.text == "Medium")
        {
            difficulty = 3;
            difficultyTextMesh.text = "Hard";
        }
        else if (difficultyTextMesh.text == "Hard")
        {
            difficulty = 1;
            difficultyTextMesh.text = "Easy";
        }

        PlayerPrefs.SetInt("difficulty", difficulty);
        PlayerPrefs.Save();
    }
}
