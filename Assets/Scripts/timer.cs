using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class timer : MonoBehaviour
{
    private float timePassed;
    private bool timerActive;
    private TMP_Text mainTimer;
    // time granularity of program is in 10 minutes
    // select how many seconds represent one window of 10 minutes
    public float secondsInTenMinutes;  
    // first hour of a working day. 24 hour format. 8.5 represents 8:30
    public float startingHour;

    // Start is called before the first frame update
    void Start()
    {
        // 7 seconds per 10 minutes makes in game 8 hours == 5.5 minutes irl
        if (secondsInTenMinutes <= 0){
        secondsInTenMinutes = 7.0f; 
        }
        if (startingHour < 0){
            startingHour = 8.0f;
        }
        timerActive = true;
        timePassed = 0;

        mainTimer = GameObject.Find("timeText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timerActive){
            timePassed += Time.deltaTime;
        }
        DisplayTime();
    }

    private void DisplayTime(){
        int minutes;
        int hours;

        minutes = ((int)(this.timePassed / secondsInTenMinutes)) * 10;
        minutes +=(int) startingHour * 60;  

        hours = minutes / 60;
        minutes = minutes % 60;
        mainTimer.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, "00");
    }

    public void StartTimer(){
        this.timerActive = true;
    }

    public void StopTimer(){
        this.timerActive = false;
    }

}
