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
        SceneManager.LoadScene("SampleScene");
    }

    private void loadDay(){
        var directory = new DirectoryInfo(Application.persistentDataPath);
        var files = directory.GetFiles().OrderByDescending(f => f.LastWriteTime);
        
        string savedDataText = File.ReadAllText(directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First().FullName);
        Save savedData = JsonConvert.DeserializeObject<Save>(savedDataText);
        NestedStatus nestedStatus = savedData.Status;

        Status newStatus = JsonConvert.DeserializeObject<Status>(statusFile.text);

        Debug.Log(newStatus.Vehicle[nestedStatus.Vehicle]);
        Debug.Log(newStatus.Health[nestedStatus.Health]);
        Debug.Log(newStatus.SocialStatus[nestedStatus.SocialStatus]);
        Debug.Log(newStatus.Living[nestedStatus.Living]);
    }
}
