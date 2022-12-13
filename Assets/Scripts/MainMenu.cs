using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor;

public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void newGame()
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
            var files = Directory.EnumerateFiles(folderPath);
            foreach (var file in files)
            {
                File.Delete(file);
            }

        }
            catch (System.Exception)
        {
            
            //throw;
        }

        // 2. load game 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        }
    }
}
