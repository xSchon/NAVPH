using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public TextAsset jsonFile;
    // Start is called before the first frame update
    void Start()
    {
        JObject getResult = JObject.Parse(jsonFile.text);
        Debug.Log(getResult.GetType());
        Debug.Log(getResult["1"]);
        Debug.Log(getResult);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}