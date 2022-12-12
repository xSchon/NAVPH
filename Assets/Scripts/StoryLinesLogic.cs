using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending{
    public string type;
    public string[] messages;
}

public class StoryLine{
    public bool displayed = false;
    public bool[] succesful_storyline = new bool[]{};
    public int minimum_passed = 0;
    public Ending ending; 
}

public class StoryLinesLogic : MonoBehaviour
{
    public TextAsset storylinesJson;
    private Dictionary<string, StoryLine> storyLines;
    

    void Start()
    {
        storyLines = JsonConvert.DeserializeObject<Dictionary<string, StoryLine>>(storylinesJson.text);
    }
    public void CheckStoryLines(Dictionary<int, List<bool>> newResults)
    {
        foreach(int newKey in newResults.Keys.ToArray())
        {
            Debug.Log(newKey);
            Debug.Log(newResults[newKey]);
        }
    }

}