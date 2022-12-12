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
    public List<bool> succesful_storyline = new List<bool>();
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
    
    public Dictionary<string, List<bool>> UpdateStoryLines(Dictionary<string, List<bool>> newValues, Dictionary<string, List<bool>> existingValues)
    {
        foreach (string checkKey in newValues.Keys.ToArray())
        {
            if (existingValues.ContainsKey(checkKey))
            {
                //List<bool> joinedLists;
                existingValues[checkKey].AddRange(newValues[checkKey]);

                Debug.Log("NEW EXISTING VALUES");
                for(int i=0;i<existingValues[checkKey].Count;i++)
                {
                    Debug.Log(existingValues[checkKey][i]);
                }
            }
            else
            {
                existingValues.Add(checkKey, newValues[checkKey]);
            }
        }
        CheckStoryLines(existingValues);
        return existingValues; 
    }

    public void CheckStoryLines(Dictionary<string, List<bool>> storyLinesEval)
    {
        foreach(string newKey in newResults.Keys.ToArray())
        {
            Debug.Log(newKey);
            Debug.Log(newResults[newKey]);
        }
    }

}