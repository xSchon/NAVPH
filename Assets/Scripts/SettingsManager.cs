using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI soundTextMesh;
    [SerializeField] private TMPro.TextMeshProUGUI difficultyTextMesh;

    void Start()
    {
        if (AudioListener.volume == 0)
        {
            soundTextMesh.text = "OFF";
        }
        else
        {
            soundTextMesh.text = "ON";
        }
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
    }

    public void SetDifficulty()
    {
        if (difficultyTextMesh.text == "Easy")
        {
            difficultyTextMesh.text = "Medium";
        }
        else if (difficultyTextMesh.text == "Medium")
        {
            difficultyTextMesh.text = "Hard";
        }
        else if (difficultyTextMesh.text == "Hard")
        {
            difficultyTextMesh.text = "Easy";
        }

    }
}
