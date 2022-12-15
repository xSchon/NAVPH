using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private float timePassed;
    private float sinceLast = 0.0f;
    private bool timerActive;
    public GameObject gameLogicObject;
    private GameLogic gameLogicScript;
    public GameObject timeTextObject;
    private TMP_Text mainTimer;
    // time granularity of program is in 10 minutes
    // select how many seconds represent one window of 10 minutes
    public float secondsInTenMinutes = 8.0f;
    // first hour of a working day. 24 hour format. 8.5 represents 8:30
    public float startingHour = 7.0f;
    public float endingHour = 15.0f;



    void Start()
    {
        // 7 seconds per 10 minutes makes in game 8 hours == 5.5 minutes irl
        Debug.Log(startingHour);
        if (secondsInTenMinutes <= 0)
        {
            secondsInTenMinutes = 7.0f;
        }
        if (startingHour < 0)
        {
            startingHour = 8.0f;
        }
        timerActive = true;
        timePassed = 0;

        mainTimer = timeTextObject.GetComponent<TextMeshProUGUI>();
        gameLogicScript = gameLogicObject.GetComponent<GameLogic>();
        UpdateTime();
    }

    void Update()
    {
        if (timerActive)
        {
            timePassed += Time.deltaTime;
            sinceLast += Time.deltaTime;
        }

        if (sinceLast >= secondsInTenMinutes)
        {
            // There is no need to call time-dependend functions every second.
            UpdateTime();
            sinceLast = 0.0f;
        }
    }

    private void UpdateTime()
    {
        int minutes;
        int hours;

        minutes = ((int)(this.timePassed / secondsInTenMinutes)) * 10;
        minutes += (int)(startingHour * 60);
        if (minutes > this.endingHour * 60)
        {
            Debug.Log("End of the day");
            gameLogicScript.EndDay();
            return;
        }

        hours = minutes / 60;
        minutes = minutes % 60;
        string newTime = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, "00");
        mainTimer.text = newTime;

        gameLogicScript.checkMessages(mmHHtoMinutes(newTime));
    }

    public void StartTimer()
    {
        this.timerActive = true;
        mainTimer.color = new Color(0.267f, 0.773f, 0.325f, 1.000f);
    }

    public void StopTimer()
    {
        this.timerActive = false;
        mainTimer.color = new Color(0.130f, 0.349f, 0.160f, 1.000f);
    }

    public int getCurrentMinutes()
    {
        return ((int)(this.timePassed / secondsInTenMinutes)) * 10;
    }

    public void SetStartingHour(string newStarting)
    {
        this.startingHour = mmHHtoMinutes(newStarting) / 60.0f;
    }

    public void SetEndingHour(string newEnding)
    {
        this.endingHour = mmHHtoMinutes(newEnding) / 60.0f;
    }

    public int mmHHtoMinutes(string timeHHMM)
    {
        int tmpMins;
        tmpMins = (int)TimeSpan.Parse(timeHHMM).TotalMinutes;
        return tmpMins;
    }
}
