using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheEnd : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI letterTextMesh;

    // to do:
    // load the status from save file 
    // load the storyline evaluation json file
    // load the ending text from the json file based on the status

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Return()
    {
        SceneManager.LoadScene("Menu");
    }
}
