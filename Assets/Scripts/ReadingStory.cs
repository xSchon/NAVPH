using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class ReadingStory : MonoBehaviour
{
    private string messageAuthor;
    private string[] messageText; 
    TMP_Text textField;

    private float sinceLast = 0.0f;
    public float secondsForText = 4.0f;
    private int currentChunk = 0;
    private bool readActive = false;
    // Start is called before the first frame update
    void Start()
    {
        if (secondsForText <= 0.0f){
            secondsForText = 4.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(readActive){
            sinceLast += Time.deltaTime;

            if (sinceLast > secondsForText){
                sinceLast = 0.0f;
                readNext();
            }
        }
    }

    public void readNext(){
        if(currentChunk < this.messageText.Length){
            this.textField.text = this.messageText[currentChunk]; 
            currentChunk++;
        } else {
            readActive = false;
            this.textField.text = "";
            GameObject.Find("WaveButton").GetComponent<WaveClicked>().FinishedRead();
        }

    }
    public void DisplayConversation(string author, string[] mainText, TMP_Text textField){
        this.messageAuthor = author;
        this.messageText = mainText;
        this.textField = textField;
        textField.text = "Radio communication of " +author+".";
        sinceLast = 0.0f;
        readActive = true;
        currentChunk = 0;
    }
}
