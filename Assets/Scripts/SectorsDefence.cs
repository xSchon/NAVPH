/* Maintain the defence of sectors at the map and save information about people passing */
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SectorsDefence : MonoBehaviour
{
    public int sectorsAmount = 2;
    private int[] selectedSectors;
    public Dictionary<int, List<DefendableSector>> toDefend = new Dictionary<int, List<DefendableSector>>();
    public Dictionary<string, List<bool>> storyLines = new Dictionary<string, List<bool>>();
    public TMP_Text defendingSectorsText;
    public List<Image> sectorsAll; // ORDERED sectors 1 to 9
    public SusBar susBar;
    public PopUpMessage messagePop;

    void Start()
    {
        if (sectorsAmount < 0 || sectorsAmount > 9)
        {
            // if not set properly in unity, protect program from breaking
            sectorsAmount = 2;
        }
        selectedSectors = new int[sectorsAmount];

        for (int i = 0; i < selectedSectors.Length; i += 1)
        {
            selectedSectors[i] = 0;
        }

    }

    public void NoteSector(string sectorName)
    {
        int sectorNumber;
        int dropSector = 0;
        sectorNumber = int.Parse(sectorName[^1].ToString());  

        if (!this.selectedSectors.Contains(sectorNumber))
        {    // if selected sector is not among selected already
            dropSector = selectedSectors[^1];                // drop the sector that is selected for longest

            for (int i = selectedSectors.Length - 1; i > 0; i--)
            {
                selectedSectors[i] = selectedSectors[i] = selectedSectors[i - 1];
            }
            selectedSectors[0] = sectorNumber;
        }

        ChangeText(string.Join(", ", selectedSectors));
        if (dropSector != 0)
        {
            VisibleSector("Sector" + dropSector, false);
        }
        VisibleSector(sectorName, true);

    }

    private void ChangeText(string sectorsNumbers)
    {
        defendingSectorsText.text = "Defending sectors: " + sectorsNumbers;
    }

    private void VisibleSector(string sectorName, bool setVisible)
    {
        // if decolor is false, then set to invisible (A = 0), otherwise set to visible (A = 0.33)
        int selectedSector = (int)Char.GetNumericValue(sectorName[sectorName.Length - 1]) - 1;
        Color objColor;
        objColor = sectorsAll[selectedSector].color;
        if (setVisible)
        {
            objColor[3] = 0.33f;
        }
        else
        {
            objColor[3] = 0.00f;
        }
        sectorsAll[selectedSector].color = objColor;
    }

    public void NewToDefend(int defendAt, DefendableSector defSect)
    {
        // add information about sector defence at certain time
        if (this.toDefend.ContainsKey(defendAt))
        {
            List<DefendableSector> addable = this.toDefend[defendAt];
            addable.Add(defSect);
            this.toDefend[defendAt] = addable;
        }
        else
        {
            this.toDefend.Add(defendAt, (new List<DefendableSector> { defSect }));
        }
    }

    public void CheckSectors(int currentTime)
    {
        // Find if there is a need to protect a sector at given time.
        bool passed;
        if (this.toDefend.ContainsKey(currentTime))
        {
            foreach (DefendableSector defSec in this.toDefend[currentTime])
            {
                if (!this.selectedSectors.Contains(defSec.sectorNum))
                {
                    messagePop.AddWarning(defSec.sectorNum, defSec.susPunish);
                    susBar.InfluenceSus(defSec.susPunish);

                    passed = true;
                }
                else
                {  // if protected
                    messagePop.AddApproval(defSec.sectorNum);
                    passed = false;
                }

                // save information about passing to storylines
                string storyStringNum = defSec.storyNum.ToString();
                if (this.storyLines.ContainsKey(storyStringNum))
                {
                    List<bool> toAdd = this.storyLines[storyStringNum];
                    toAdd.Add(passed);
                    this.storyLines[storyStringNum] = toAdd;
                }
                else
                {
                    this.storyLines.Add(storyStringNum, (new List<bool> { passed }));
                }
            }
        }
    }

    public Dictionary<string, List<bool>> GetStoryLines()
    {
        return this.storyLines;
    }


}
