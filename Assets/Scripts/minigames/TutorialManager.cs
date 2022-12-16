using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public Minigame1 script;
    public Main script2;
    public TextEntered script3;
    public Canvas tutorial;
    public void EndTutorialMinigame1()
    {
        script.SpawNewBrick();
        tutorial.gameObject.SetActive(false);
    }

    public void EndTutorialMinigame2()
    {
        script2.UnfreezeTime();
        tutorial.gameObject.SetActive(false);
    }

    public void EndTutorialMinigame3()
    {
        script3.UnfreezeTime();
        tutorial.gameObject.SetActive(false);
    }

    public void EndTutorialMinigame4()
    {
        tutorial.gameObject.SetActive(false);
    }
}
