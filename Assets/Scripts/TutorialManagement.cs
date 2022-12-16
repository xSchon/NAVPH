/* Script managing tutorial screen in game, before the first level */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManagement : MonoBehaviour
{
    public GameObject[] tutorialsScreen;
    private int activeScreen = 0; 
    void Start()
    {
        foreach (GameObject tutS in tutorialsScreen)
        {
            tutS.SetActive(false);
        }
        tutorialsScreen[activeScreen].SetActive(true);
    }

    public void NextScreen(){
        // check if it was not the last screen
        if (activeScreen == tutorialsScreen.Length - 1)
        {
            EndTutorial();
            return;
        }
        tutorialsScreen[activeScreen].SetActive(false);
        activeScreen++;
        tutorialsScreen[activeScreen].SetActive(true);
    }

    public void EndTutorial()
    {
        Debug.Log("Ending tutorial, starting first day");
        SceneManager.LoadScene("OfficeScene");
    }
}
