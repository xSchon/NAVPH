/* Script for management of pop up message based on border crossing people. 
   Loads message, shows pop up and makes it fade away. */
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpMessage : MonoBehaviour
{
    public float FadeRatio = 0.25f;
    public TMP_Text messageText;
    public TMP_Text messageEvaluation;
    private List<int> affectedSectors = new List<int>();
    private List<int> affectedAmount = new List<int>();
    public TextAsset warningMessages;
    public TextAsset approvalMessages;

    void Start()
    {
        // make message disapear
        var tmp = gameObject.GetComponent<Image>().color;
        tmp.a = 0.0f;
        gameObject.GetComponent<Image>().color = tmp;

        tmp = messageText.color;
        tmp.a = 0.0f;
        messageText.color = tmp;

        tmp = messageEvaluation.color;
        tmp.a = 0.0f;
        messageEvaluation.color = tmp;
    }

    void Update()
    {
        if (gameObject.GetComponent<Image>().color.a > 0.0f)
        {
            FadeColor();
        }
        else
        {
            CheckPopUp();
        }
    }

    private void FadeColor()
    {
        // pop up message is slowly disappearing from vision with its text too
        var tmp = gameObject.GetComponent<Image>().color;
        tmp.a -= FadeRatio * Time.deltaTime;
        gameObject.GetComponent<Image>().color = tmp;

        tmp = messageText.color;
        tmp.a -= FadeRatio * Time.deltaTime;
        messageText.color = tmp;

        tmp = messageEvaluation.color;
        tmp.a -= FadeRatio * Time.deltaTime;
        messageEvaluation.color = tmp;
    }

    private void CheckPopUp()
    {
        // Check if any popup is ready to be processed
        if (affectedSectors.Count > 0)
        {  // if items in list
            var tmp = messageEvaluation.color;

            if (affectedAmount[0] == -1)
            {  // if approval
                string[] lines = approvalMessages.text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                messageText.text = lines[UnityEngine.Random.Range(0, lines.Length)];
                messageEvaluation.text = "Sector " + affectedSectors[0];

                tmp = new Color(0.0f, 1.0f, 0.0f, 1.0f);

            }
            else
            { // if warning
                string[] lines = warningMessages.text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                messageText.text = lines[UnityEngine.Random.Range(0, lines.Length)];
                messageEvaluation.text = "Sec. " + affectedSectors[0] + ", +" + affectedAmount[0] + " susBar";

                tmp = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }

            messageEvaluation.color = tmp;
            affectedSectors.RemoveAt(0);
            affectedAmount.RemoveAt(0);

            tmp = gameObject.GetComponent<Image>().color;
            tmp.a = 1.0f;
            gameObject.GetComponent<Image>().color = tmp;

            tmp = messageText.color;
            tmp.a = 1.0f;
            messageText.color = tmp;
        }
    }

    public void AddWarning(int sectorNr, int punish)
    {
        affectedSectors.Add(sectorNr);
        affectedAmount.Add(punish);
    }

    public void AddApproval(int sectorNr)
    {
        affectedSectors.Add(sectorNr);
        affectedAmount.Add(-1);
    }
}
