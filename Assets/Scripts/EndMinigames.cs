using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMinigames : MonoBehaviour
{
    public Camera minigameCamera;
    public string minigameName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void endMinigame(){
        Debug.Log("XD scena je: " +  SceneManager.GetActiveScene().name);

        int countLoaded = SceneManager.sceneCount;
        int parent_scene_id = -1;
        for (int i = 0; i < countLoaded; i++)
        {
            
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != minigameName){
                parent_scene_id = i;
            }
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneAt(parent_scene_id));
        minigameCamera.enabled = false;

    }
}
