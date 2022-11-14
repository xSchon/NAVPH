using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class susBar : MonoBehaviour
{
    private Slider slider;
    private float standardChangeValue; 
    private GameObject sliderFilling;
    // Start is called before the first frame update
    void Start()
    {
        standardChangeValue = 5.0f;
        slider = gameObject.GetComponent<Slider>();
        sliderFilling = GameObject.Find("susBarFill");
    }

    // Update is called once per frame
    void Update()
    {
               
    }

    private void controlSus(){
        Debug.Log(slider.value);
        increaseSus(0.5f);

        Color newBarColor = new Color(1, 1, 1);
        if(slider.value == 0){
            newBarColor = new Color(0, 0, 0);
        } else if (slider.value < 30){
            newBarColor = new Color(0, 1, 0);
        } else if (slider.value < 70){
            newBarColor = new Color(1, 0.64f, 0);
        } else if (slider.value <= 100){
            newBarColor = new Color(1, 0, 0);
            if (slider.value == 100){
                Debug.Log("You have lost the game");
            } 
        } else{
            Debug.Log("Something went wrong and slider value is "+slider.value);
        }
        sliderFilling.GetComponent<Image>().color = newBarColor;
    }

    public void increaseSus(){
        slider.value += standardChangeValue;
        controlSus(); 
    }

    public void increaseSus(float customChangeValue){
        slider.value += customChangeValue;
        controlSus(); 
    }

    public void decreaseSus(){
        slider.value -= standardChangeValue;
        controlSus(); 
    }

    public void decreaseSus(float customChangeValue){
        slider.value -= customChangeValue;
        controlSus(); 
    }
}
