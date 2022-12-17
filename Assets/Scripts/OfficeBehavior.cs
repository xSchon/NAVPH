/* Script checking clicking on elements and triggering actions accordingly */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class OfficeBehavior : MonoBehaviour
{
    public GameObject radioScreen;
    public GameObject mapScreen;
    public GameObject pauseScreen;
    private string[] radiosNames = new string[3] { "Radio1", "Radio2", "Radio3" };
    private string mapObjectName = "Map";
    public AudioSource radioStatic;
    public WaveClicked waveClicked;

    void Start()
    {
        // screens can be either disabled via SetActive to save resources or
        // made invisible via .enabled = false if they need to be accessed 
        radioScreen.GetComponent<Canvas>().enabled = false;
        mapScreen.GetComponent<Canvas>().enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && (!radioScreen.GetComponent<Canvas>().enabled || !radioScreen.activeSelf)
             && (!mapScreen.GetComponent<Canvas>().enabled || !mapScreen.activeSelf) && !pauseScreen.activeSelf)
        {
            // if left button pressed AND gui disabled
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                string colliderHit = hit.collider.gameObject.name;
                if (this.radiosNames.Contains(colliderHit))
                { // if clicked on the radio 
                    int selectedRadio = int.Parse(colliderHit[colliderHit.Length - 1].ToString());
                    radioScreen.SetActive(true);
                    radioStatic.Play();
                    radioScreen.GetComponent<Canvas>().enabled = true;
                    waveClicked.loadScene(selectedRadio);
                }

                else if (colliderHit == mapObjectName)
                {
                    mapScreen.GetComponent<Canvas>().enabled = true;
                    mapScreen.SetActive(true);
                }
                else
                {
                    Debug.Log("Object clicked on was:" + colliderHit);
                }
            }
        }
    }
}

