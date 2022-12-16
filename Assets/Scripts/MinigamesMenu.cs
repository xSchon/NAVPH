/* Load chosen minigames based on button click on them */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigamesMenu : MonoBehaviour
{

    public void PlayMinigame1()
    {
        SceneManager.LoadScene("minigame-1");
    }

    public void PlayMinigame2()
    {
        SceneManager.LoadScene("minigame-2");
    }

    public void PlayMinigame3()
    {
        SceneManager.LoadScene("minigame-3");
    }

    public void PlayMinigame4()
    {
        SceneManager.LoadScene("minigame-4");
    }
}
