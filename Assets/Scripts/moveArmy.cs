using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class moveArmy : MonoBehaviour
{
    private GameObject sectors;
    private TMP_Text subtitles;
    // Start is called before the first frame update
    void Start()
    {
        sectors = GameObject.Find("selectedSectors");
        sectors.GetComponent<sectorsDeffence>().changeText();

        //Debug.Log(sectors);
        //sectors.GetComponent<sectorsDeffence>().changeText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectSector()
    {
        Debug.Log("Presunul si armadu na "+ this.name);
    }
}
