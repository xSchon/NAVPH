using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radioBehavior : MonoBehaviour
{
    private GameObject radioScreen;
    private GameObject mapScreen;
    private string radioObjectName = "Radio";
    private string mapObjectName = "Map";

    // Start is called before the first frame update
    void Start()
    {
      radioScreen = GameObject.Find("radioScreen");
      radioScreen.SetActive(false);
      mapScreen = GameObject.Find("mapScreen");
      mapScreen.SetActive(false);
    }

    // Update is called once per frame
 void Update(){
   if (Input.GetMouseButtonDown(0) && !radioScreen.activeSelf && !mapScreen.activeSelf){ // if left button pressed AND gui disabled
     Debug.Log("xdd");
     Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
     RaycastHit hit;

     if (Physics.Raycast(ray, out hit)){
        if(hit.collider.gameObject.name == radioObjectName){ // if clicked on the radio 
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

