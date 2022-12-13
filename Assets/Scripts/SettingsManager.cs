using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI soundTextMesh;

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
}
