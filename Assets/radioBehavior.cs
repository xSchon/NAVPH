using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radioBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
 void Update(){
   if (Input.GetMouseButtonDown(0)){ // if left button pressed...
     Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
     RaycastHit hit;
     if (Physics.Raycast(ray, out hit)){
        Debug.Log("xdd");
     }
   }
 }

}

