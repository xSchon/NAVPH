/* Screen shown when player is reading the messages in the radio */
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class ReadingStory : MonoBehaviour
{
    private string messageAuthor;
    private string[] messageText;
    private TMP_Text textField;
    public WaveClicked waveClicked;
    private float sinceLast = 0.0f;
    public float secondsForText = 4.0f;
    private int currentChunk = 0;
    private bool readActive = false;

    void Update()
    {
        // read message if there is one to read, part by part
        if (readActive)
        {
            sinceLast += Time.deltaTime;

            if (sinceLast > secondsForText)
            {
                ReadNext();
            }
        }
    }

    public void ReadNext()
    {
        // show message
        sinceLast = 0.0f;
        if (currentChunk < this.messageText.Length)
        {
            this.textField.text = this.messageText[currentChunk];
            currentChunk++;
        }
        else  // if whole message has been shown
        {
            readActive = false;
            this.textField.text = "";
            waveClicked.FinishedRead();
        }

    }
    public void DisplayConversation(string author, string[] mainText, TMP_Text textField)
    {
        // load information about the message
        this.messageAuthor = author;
        this.messageText = mainText;
        this.textField = textField;
        textField.text = "Radio communication of " + author + ".";
        sinceLast = 0.0f;
        readActive = true;
        currentChunk = 0;
    }
}
