using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SusBar : MonoBehaviour
{
    private Slider slider;
    private float standardChangeValue = 5.0f; 
    public GameObject sliderFilling;

    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        controlSus();
    }


    private void controlSus()
    {
        Color newBarColor = new Color(1, 1, 1);
        if(slider.value == 0){
            newBarColor = new Color(0, 0, 0);
        } else if (slider.value < 30){
            newBarColor = new Color(0, 1, 0);
        } else if (slider.value < 70){
            newBarColor = new Color(1, 0.64f, 0);
        } else if (slider.value <= 100){
            newBarColor = new Color(1, 0, 0);
            if (slider.value >= 100){
                Debug.Log("You have lost the game");
                SceneManager.LoadScene("Ending");
            } 
        } else{
            Debug.Log("Something went wrong and slider value is "+slider.value);
        }
        sliderFilling.GetComponent<Image>().color = newBarColor;
    }

    public void IncreaseSus(){
        slider.value += standardChangeValue;
        controlSus(); 
    }
    public void DecreaseSus(){
        slider.value -= standardChangeValue;
        controlSus(); 
    }

    public void InfluenceSus(float customChangeValue){
        slider.value += customChangeValue;
        controlSus(); 
    }

    public float GetSusValue()
    {
        return slider.value;
    }

    public void SetSusValue(float newValue)
    {
        slider.value = newValue;
        controlSus();
    }
}
