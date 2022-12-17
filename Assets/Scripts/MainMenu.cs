/* Main menu of the game an it's buttons */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    string[] files = null;
    string folderPath;
    public GameObject dialogueWindow;
    public GameObject continueButton;

    void Start()
    {
        folderPath = Application.persistentDataPath;
        files = Directory.GetFiles(folderPath);
        if (files.Length == 0)
        {
            continueButton.SetActive(false);
        }
        dialogueWindow.SetActive(false);
    }

    public void PlayGame() // Continue Game if save files exist, othwerise continue button is disabled 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        dialogueWindow.SetActive(true);
    }

    public void NewGameConfirmed()
    {
        if (files.Length > 0)
        {
            // 1. delete save file
            try
            {
                string folderPath = Application.persistentDataPath;
                IEnumerable<string> files = Directory.EnumerateFiles(folderPath);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
                // 2. load new game 
                SceneManager.LoadScene("Video");
            }
            catch (System.Exception)
            {
                Debug.Log("System exception in deciding");
            }
        }

        else // no save file was found, we can start a new game 
        {
            // 2. load new game, starting from tutorial
            SceneManager.LoadScene("Video");
        }
    }

    public void NewGameDenied()
    {
        dialogueWindow.SetActive(false);
    }
}
