using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class JSONLoader : MonoBehaviour
{
    public TextAsset jsonFile;
    JObject getResult;

    void Start()
    {
        getResult = JObject.Parse(jsonFile.text);
        Debug.Log(getResult["1"]);
    }

  /*  public JObject getJson(){
        return this.getResult();
    } */
}
