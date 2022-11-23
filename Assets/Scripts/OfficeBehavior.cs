using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class OfficeBehavior : MonoBehaviour
{
    private GameObject radioScreen;
    private GameObject mapScreen;
    private GameObject pauseScreen;
    private string[] radiosNames = new string[3]{"Radio1", "Radio2", "Radio3"};
    private string mapObjectName = "Map";

    // Start is called before the first frame update
    void Start()
    {
      // GUI alternative if needs to be accessed via other scripts as well:
      // radioScreen.GetComponent<Canvas>().enabled = true;
      radioScreen = GameObject.Find("RadioScreen");
      radioScreen.SetActive(false);
      mapScreen = GameObject.Find("MapScreen");
      mapScreen.SetActive(false);
      pauseScreen = GameObject.Find("PauseScreen");
    }

    // Update is called once per frame
 void Update(){
   if (Input.GetMouseButtonDown(0) && !radioScreen.activeSelf && !mapScreen.activeSelf && !pauseScreen.activeSelf){ 
    // if left button pressed AND gui disabled
     Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
     RaycastHit hit;

     if (Physics.Raycast(ray, out hit)){
        if(this.radiosNames.Contains(hit.collider.gameObject.name)){ // if clicked on the radio 
            radioScreen.SetActive(true);
        }
        else if(hit.collider.gameObject.name == mapObjectName){
            mapScreen.SetActive(true);
        }
        else{
            Debug.Log(hit.collider.gameObject.name);
        }
     }
   }
 }
}

