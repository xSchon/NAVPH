using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigamesMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        SceneManager.LoadScene("Minigame4");
    }
}
