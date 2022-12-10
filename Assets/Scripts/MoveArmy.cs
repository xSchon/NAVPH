using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveArmy : MonoBehaviour
{
    private GameObject sectors;
    private TMP_Text subtitles;
    // Start is called before the first frame update
    void Start()
    {
        sectors = GameObject.Find("selectedSectors");
    }

    public void selectSector()
    {
        sectors.GetComponent<SectorsDeffence>().noteSector(this.name);
    }
}
