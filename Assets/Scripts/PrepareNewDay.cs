using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;

public class PrepareNewDay : MonoBehaviour
{
    private Scene mainScene;
    public TextAsset statusFile;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeScene(){
        SceneManager.LoadScene("OfficeScene");
    }

    private void loadDay(){
        var directory = new DirectoryInfo(Application.persistentDataPath);
        var files = directory.GetFiles().OrderByDescending(f => f.LastWriteTime);
        
        string savedDataText = File.ReadAllText(directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First().FullName);
        Save savedData = JsonConvert.DeserializeObject<Save>(savedDataText);
        NestedStatus nestedStatus = savedData.status;

        Status newStatus = JsonConvert.DeserializeObject<Status>(statusFile.text);
    }
}
