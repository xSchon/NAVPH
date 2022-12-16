/* Script for informing sector it's been selected */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveArmy : MonoBehaviour
{
    public SectorsDefence sectors;

    public void SelectSector()
    {
        sectors.noteSector(this.name);
    }
}
