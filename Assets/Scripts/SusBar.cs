/* Script managing suspicion bar - visualization of the value, storing it and overall susbar control */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SusBar : MonoBehaviour
{
    public Slider slider;
    private float standardChangeValue = 5.0f;
    public GameObject sliderFilling;

    void Start()
    {
        ControlSus();
    }

    private void ControlSus()
    {
        Color newBarColor = new Color(1, 1, 1);
        switch (slider.value)
        {
            case 1:
                newBarColor = new Color(0, 0, 0);
                break;
            case < 30:
                newBarColor = new Color(0, 1, 0);
                break;
            case < 70:
                newBarColor = new Color(1, 0.64f, 0);
                break;
            case < 100:
                newBarColor = new Color(1, 0, 0);
                break;
            case >= 100:
                string endingText = "";
                Debug.Log("You have lost the game");
                endingText += "That was not a good service, comrade... \n\n";
                endingText += "You have failed at your job...to many people escaped our mighty country. You will be properly punished for that.";

                PlayerPrefs.SetString("endingText", endingText);
                PlayerPrefs.Save();
                SceneManager.LoadScene("Ending");
                break;
        }
        sliderFilling.GetComponent<Image>().color = newBarColor;
    }

    public void IncreaseSus()
    {
        slider.value += standardChangeValue;
        ControlSus();
    }
    public void DecreaseSus()
    {
        slider.value -= standardChangeValue;
        ControlSus();
    }

    public void InfluenceSus(float customChangeValue)
    {
        slider.value += customChangeValue;
        ControlSus();
    }

    public float GetSusValue()
    {
        return slider.value;
    }

    public void SetSusValue(float newValue)
    {
        slider.value = newValue;
        ControlSus();
    }
}
