using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextEntered : MonoBehaviour
{
    private TMP_InputField output_text;
    private TMP_Text morse_field;
    private TMP_Text time_field;
    private TMP_Text popup;
    private GameObject button;
    private Canvas popup_canvas;
    private bool game_end = false;
    public float timeRemaining;
    public string morse_code;
    public string ascii_code;
    public string lostText = "Prehrali ste, velmi nas to mrzi a snad vam to vyjde nabuduce :)";
    public string winText = "Vylustili ste upsne spravu! Dakujeme za vasu obetavu pracu!";
    // Start is called before the first frame update
    void Start()
    {
        output_text = gameObject.GetComponent<TMP_InputField>();
        time_field = GameObject.Find("time_field").GetComponent<TextMeshProUGUI>();
        morse_field = GameObject.Find("source_text").GetComponent<TextMeshProUGUI>();
        popup = GameObject.Find("popup").GetComponent<TextMeshProUGUI>();

        popup_canvas = GameObject.Find("popup_canvas").GetComponent<Canvas>();
        popup_canvas.enabled = false;
        morse_field.text = morse_code;

        Debug.Log("Start metoda");
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining < 0){
            popup_canvas.enabled = true;
            popup.text = lostText;
        } else if (!game_end) {
            timeRemaining -= Time.deltaTime;
            time_field.text = timeRemaining.ToString();
        }
    }

    public void TextSubmitted(){
        Debug.Log("Zadali ste text -> " + output_text.text.ToLower());

        if (output_text.text.ToLower() == ascii_code){
            popup_canvas.enabled = true;
            popup.text = winText;
            game_end = true;
        }
    }

    public void SetAttributes(string morse_text, string ascii_text, float time){
        this.morse_code = morse_text;
        this.ascii_code = ascii_text;
        this.timeRemaining = time;

        Debug.Log("Set Attribttues metoda");
    }
}
