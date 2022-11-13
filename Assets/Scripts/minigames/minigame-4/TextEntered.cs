using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextEntered : MonoBehaviour
{
    public TMP_InputField output_text;

    // Start is called before the first frame update
    void Start()
    {
        output_text = gameObject.GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TextSubmitted(){
        Debug.Log("Zadali ste text -> " + output_text.text);
    }
}
