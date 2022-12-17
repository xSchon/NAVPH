/* Main menu of the game an it's buttons */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class MainMenu : MonoBehaviour
{
    string folderPath;
    public GameObject dialogueWindow;
    public GameObject continueButton;

    void Start()
    {
        DirectoryInfo directory = new DirectoryInfo(Application.persistentDataPath);
        IEnumerable<FileInfo> files = directory.GetFiles();
        string[] namesSkip = {"prefs", "Player.log", "Player-prev.log"};    
        files = files.OrderByDescending(f => f.LastWriteTime).Where(f => !namesSkip.Any(f.Name.Contains));

        if (!files.Any())
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
        DirectoryInfo directory = new DirectoryInfo(Application.persistentDataPath);
        IEnumerable<FileInfo> files = directory.GetFiles();
        string[] namesSkip = {"prefs", "Player.log", "Player-prev.log"};    
        files = files.OrderByDescending(f => f.LastWriteTime).Where(f => !namesSkip.Any(f.Name.Contains));
        
        if (!files.Any())
        {
            SceneManager.LoadScene("Video");
        } else {
            dialogueWindow.SetActive(true);
        }
    }

    public void NewGameConfirmed()
    {
        DirectoryInfo directory = new DirectoryInfo(Application.persistentDataPath);
        IEnumerable<FileInfo> files = directory.GetFiles();
        string[] namesSkip = {"prefs", "Player.log", "Player-prev.log"};    
        files = files.OrderByDescending(f => f.LastWriteTime).Where(f => !namesSkip.Any(f.Name.Contains));

        if (files.Any())
        {
            // 1. delete save file
            try
            {
                foreach (FileInfo file in files)
                {
                    file.Delete();
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
