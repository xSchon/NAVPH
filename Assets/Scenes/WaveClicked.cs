using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static radioBehavior;

public class WaveClicked : MonoBehaviour
{
    private GameObject radioScreen;
    // Start is called before the first frame update
    void Start()
    {
        radioScreen = GameObject.Find("radioScreen");
        //radioScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void StartVoice(){
        Debug.Log("CLickol si na wave button :)");
    }

    public void CloseCanvas(){
        radioScreen.SetActive(false);
    }
}
