using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending
{
    public string type;
    public string[] messages;
    public string field = "none";
    public int influence = 0;
}

public class StoryLine
{
    public List<bool> succesful_storyline = new List<bool>();
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
                existingValues[checkKey].AddRange(newValues[checkKey]);
            }
            else
            {
                existingValues.Add(checkKey, newValues[checkKey]);
            }
        }
        existingValues = CheckStoryLines(existingValues);
        return existingValues;
    }

    public Dictionary<string, List<bool>> CheckStoryLines(Dictionary<string, List<bool>> storyLinesEval)
    // Check for game - ending lines.
    {
        string storyText1 = "";
        string storyText2 = "";

        foreach (string checkKey in storyLinesEval.Keys.ToArray())
        {
            if (storyLines.ContainsKey(checkKey))
            {
                if (storyLines[checkKey].succesful_storyline.SequenceEqual(storyLinesEval[checkKey]))
                // if the storyline was completed succesfully
                {
                    if (storyLines[checkKey].ending.type == "full_ending")
                    {
                        Debug.Log("It's time to end the game");
                        string endingText = "";
                        endingText += "This is the end for now...\n\n";
                        endingText += storyLines[checkKey].ending.messages[0];  

                        PlayerPrefs.SetString("endingText", endingText);
                        PlayerPrefs.SetInt("storyLinesEnd", 1);
                        PlayerPrefs.Save();
                        return storyLinesEval;
                    }
                    else if (storyLines[checkKey].ending.type == "end_day_message")
                    {
                        for (int i = 0; i < storyLines[checkKey].ending.messages.Length; i++)
                        {
                            if (storyText2 == "")
                            {
                                storyText2 = storyLines[checkKey].ending.messages[i];
                            }
                            else if (storyText1 == "")
                            {
                                storyText1 = storyLines[checkKey].ending.messages[i];
                            }
                            storyLinesEval[checkKey].Add(false);

                            gameObject.GetComponent<GameLogic>().StatusFromStoryLines(storyLines[checkKey].ending.field, storyLines[checkKey].ending.influence);
                        }
                    }
                }
            }

        }

        PlayerPrefs.SetInt("storyLinesEnd", 0);
        PlayerPrefs.SetString("storyText1", storyText1);
        PlayerPrefs.SetString("storyText2", storyText2);
        PlayerPrefs.Save();
        return storyLinesEval;
    }
}