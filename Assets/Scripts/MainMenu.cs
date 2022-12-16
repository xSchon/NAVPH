using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor;

public class MainMenu : MonoBehaviour
{
    string[] files = null;
    string folderPath;
    public GameObject continueButton;

    void Start()
    {
        folderPath = Application.persistentDataPath;
        files = Directory.GetFiles(folderPath);
        if (files.Length == 0) 
        {
            continueButton.SetActive(false);
        }

    }

    public void PlayGame() // Continue Game if save files exist, othwerise continue button is disabled 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void newGame()
    {
        if (files.Length > 0) 
        {
            // 1. delete save file
            bool decision = EditorUtility.DisplayDialog(
            "New Game", // title
            "Starting a new game will delete all save files. Continue?", // description
            "Yes", // OK button
            "No" // Cancel button
        );

            if (decision) 
            {
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
            
        }

        else // no save file was found, we can start a new game 
        {
            // 2. load new game 
            SceneManager.LoadScene("Video");
        }

    }
}
