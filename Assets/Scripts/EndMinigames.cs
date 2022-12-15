/* Script ending minigames and evaluating them */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMinigames : MonoBehaviour
{
    public Camera minigameCamera;
    public string minigameName;
    public string mainSceneName = "OfficeScene";

    public void EndMinigame()
    {
        int countLoaded = SceneManager.sceneCount;
        int parent_scene_id = -1;
        for (int i = 0; i < countLoaded; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != minigameName)
            {
                parent_scene_id = i;
            }
        }

        if (parent_scene_id == -1)
        {
            SceneManager.LoadScene("Menu");
            return;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(parent_scene_id));
        minigameCamera.enabled = false;

    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(mainSceneName);
    }

    public void EndMinigameSusDecrease()
    {
        PlayerPrefs.SetInt("WonMinigame", 0);
        EndMinigame();
    }
}
