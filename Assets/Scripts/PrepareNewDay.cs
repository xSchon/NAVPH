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

    public void changeScene(){
        SceneManager.LoadScene("OfficeScene");
    }

    private void loadDay(){
        DirectoryInfo directory = new DirectoryInfo(Application.persistentDataPath);
        IOrderedEnumerable<FileInfo> files = directory.GetFiles().OrderByDescending(f => f.LastWriteTime);
        
        string savedDataText = File.ReadAllText(directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First().FullName);
        Save savedData = JsonConvert.DeserializeObject<Save>(savedDataText);
        NestedStatus nestedStatus = savedData.status;

        Status newStatus = JsonConvert.DeserializeObject<Status>(statusFile.text);
    }
}
