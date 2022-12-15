using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;    


public class SectorsDefence : MonoBehaviour
{   
    public int sectorsAmount = 2; // set in Unity
    private int[] selectedSectors;
    public Dictionary<int, List<DefendableSector>> toDeffend = new Dictionary<int, List<DefendableSector>>();
    public Dictionary<string, List<bool>> storyLines = new Dictionary<string, List<bool>>();
    

    void Start()
    {
        if (sectorsAmount < 0 || sectorsAmount > 9){
            // if not set properly in unity, protect program from breaking
            sectorsAmount = 2; 
            }
        selectedSectors = new int[sectorsAmount];
        
        for(int i = 0; i < selectedSectors.Length; i += 1){
            selectedSectors[i] = 0;
        }

    }

    public void noteSector(string sectorName){
        int sectorNumber;
        int dropSector = 0;
        sectorNumber = int.Parse(sectorName[^1].ToString());  // select sector number from name sector
        
        if (!this.selectedSectors.Contains(sectorNumber)){    // if selected sector is not among selected already
            dropSector = selectedSectors[^1];                // drop the sector that is selected for longest
             
            
            for (int i = selectedSectors.Length-1; i > 0; i--){
                selectedSectors[i] = selectedSectors[i] = selectedSectors[i-1];
            }
            selectedSectors[0] = sectorNumber;
        }

        changeText(string.Join(", ", selectedSectors));
        if (dropSector != 0){
            VisibleSector("sector"+dropSector, false);
        }
        VisibleSector(sectorName, true);

    }

    private void changeText(string sectorsNumbers){
        GameObject.Find("selectedSectors").GetComponent<TextMeshProUGUI>().text = "Defending sectors: "+sectorsNumbers;
    }

    private void VisibleSector(string sectorName, bool setVisible){
        // if decolor is false, then set to invisible (A = 0), otherwise set to visible (A = 0.33)
        Color objColor;
        objColor = GameObject.Find(sectorName).GetComponent<Image>().color;
        if (setVisible){
            objColor[3] = 0.33f;
        } else {
            objColor[3] = 0.00f;
        }
        GameObject.Find(sectorName).GetComponent<Image>().color = objColor;
    }


    public void NewToDeffend(int deffendAt, DefendableSector defSect){
        if (this.toDeffend.ContainsKey(deffendAt)){
            List<DefendableSector> addable = this.toDeffend[deffendAt];
            addable.Add(defSect);
            this.toDeffend[deffendAt] = addable; 
        } else {
            this.toDeffend.Add(deffendAt, (new List<DefendableSector>{defSect}));
        }
    }

    public void CheckSectors(int currentTime){
        bool passed; 
        if (this.toDeffend.ContainsKey(currentTime)){
            WarningMessage messagePop = FindObjectOfType<WarningMessage>();

            foreach(DefendableSector deffSec in this.toDeffend[currentTime]){
                if (!this.selectedSectors.Contains(deffSec.sectorNum)){
                    messagePop.AddWarning(deffSec.sectorNum, deffSec.susPunish);
                    FindObjectOfType<SusBar>().InfluenceSus(deffSec.susPunish);

                    passed = true;
                } else {  // if protected
                    messagePop.AddApproval(deffSec.sectorNum);
                    passed = false;
                }


                string storyStringNum = deffSec.storyNum.ToString();                
                if(this.storyLines.ContainsKey(storyStringNum))
                {
                    List<bool> toAdd = this.storyLines[storyStringNum];
                    toAdd.Add(passed);
                    this.storyLines[storyStringNum] = toAdd;
                } 
                else 
                {
                    this.storyLines.Add(storyStringNum, (new List<bool>{passed}));
                }
            }
        }
    }

    public Dictionary<string, List<bool>> GetStoryLines(){
        return this.storyLines;
    }

    
}
