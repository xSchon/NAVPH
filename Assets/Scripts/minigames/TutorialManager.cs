using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public Minigame1 script;
    public Canvas tutorial;
    public void EndTutorial()
    {
        script.SpawNewBrick();
        tutorial.gameObject.SetActive(false);
    }
}
