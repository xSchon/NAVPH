/* Script managing behaviour of intro video in the game.
Plays video, allows user to skip it and finds when it ends.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer introVideo;
    public TMPro.TextMeshProUGUI skipText;

    void Start()
    {
        // Disable Skip button and fade it away
        gameObject.GetComponent<Button>().enabled = false;

        var tmp = gameObject.GetComponent<Image>().color;
        tmp.a = 0;
        gameObject.GetComponent<Image>().color = tmp;

        tmp = skipText.color;
        tmp.a = 0;
        skipText.color = tmp; ;
    }
    void Update()
    {
        if (introVideo.frame + 1 == Convert.ToInt64(introVideo.frameCount))
        {
            // if video ended
            SkipVideo();
        }


        if (gameObject.GetComponent<Image>().color.a > 0.0f)
        {
            FadeColor();
        }
        else
        {
            gameObject.GetComponent<Button>().enabled = false;
        }
    }

    public void SkipVideo()
    {
        SceneManager.LoadScene("TutorialScene");
    }
    public void ShowButton()
    {
        var tmp = gameObject.GetComponent<Image>().color;
        tmp.a = 1.0f;
        gameObject.GetComponent<Image>().color = tmp;
        gameObject.GetComponent<Button>().enabled = true;

        tmp = skipText.color;
        tmp.a = 1.0f;
        skipText.color = tmp;
    }
    private void FadeColor()
    {
        var tmp = gameObject.GetComponent<Image>().color;
        tmp.a -= 0.2f * Time.deltaTime;
        gameObject.GetComponent<Image>().color = tmp;

        tmp = skipText.color;
        tmp.a -= 0.2f * Time.deltaTime;
        skipText.color = tmp;
    }


}
