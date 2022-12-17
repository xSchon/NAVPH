/* Manager script for tutorial texts at each minigame */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public Minigame1 script;
    public Main script2;
    public TextEntered script3;
    public Canvas tutorial;

    // End tutorial at minigame-1
    public void EndTutorialMinigame1()
    {
        script.SpawNewBrick();
        tutorial.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    // End tutorial at minigame-2
    public void EndTutorialMinigame2()
    {
        script2.UnfreezeTime();
        tutorial.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    // End tutorial at minigame-3
    public void EndTutorialMinigame3()
    {
        script3.UnfreezeTime();
        tutorial.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    // End tutorial at minigame-4
    public void EndTutorialMinigame4()
    {
        tutorial.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
