using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextEntered : MonoBehaviour
{
    private TMP_InputField output_text;
    private TMP_Text time_field;
    private TMP_Text popup;
    private GameObject button;
    private Canvas popup_canvas;
    public float timeRemaining = 60;
    public string lostText = "Prehrali ste, velmi nas to mrzi a snad vam to vyjde nabuduce :)";
    // Start is called before the first frame update
    void Start()
    {
        output_text = gameObject.GetComponent<TMP_InputField>();
        time_field = GameObject.Find("time_field").GetComponent<TextMeshProUGUI>();
        popup = GameObject.Find("popup").GetComponent<TextMeshProUGUI>();

        popup_canvas = GameObject.Find("popup_canvas").GetComponent<Canvas>();
        popup_canvas.enabled = false;

        //button = GameObject.Find("Button");
        //popup.enabled = false;
        //button.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining < 0){
            popup_canvas.enabled = true;
            popup.text = lostText;
        } else {
            timeRemaining -= Time.deltaTime;
            time_field.text = timeRemaining.ToString();
        }
    }

    public void TextSubmitted(){
        Debug.Log("Zadali ste text -> " + output_text.text);
    }
}
