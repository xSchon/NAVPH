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
        Debug.Log(files.Length);
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
            // to do: add confirmation for deleting save files
            bool decision = EditorUtility.DisplayDialog(
            "New Game", // title
            "Starting a new game will delete all save files. Continue?", // description
            "Yes", // OK button
            "No" // Cancel button
        );

        if (decision) {
            try
                {
                string folderPath = Application.persistentDataPath;
                IEnumerable<string> files = Directory.EnumerateFiles(folderPath);
                foreach (string file in files)
                {
                    File.Delete(file);
                }

            }
                catch (System.Exception)
                {
                Debug.Log("System exception in deciding");
                }
            
            }
            
        }
        // 2. load game 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
