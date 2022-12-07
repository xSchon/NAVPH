using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;    


public class sectorsDeffence : MonoBehaviour
{   
    public int sectorsAmount = 2; // set in Unity
    private int[] selectedSectors;
    public Dictionary<int, List<DeffendableSector>> toDeffend = new Dictionary<int, List<DeffendableSector>>();
    public Dictionary<int, List<bool>> storyLines = new Dictionary<int, List<bool>>();
    

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
            visibleSector("sector"+dropSector, false);
        }
        visibleSector(sectorName, true);

    }

    private void changeText(string sectorsNumbers){
        GameObject.Find("selectedSectors").GetComponent<TextMeshProUGUI>().text = "Defending sectors: "+sectorsNumbers;
    }

    private void visibleSector(string sectorName, bool setVisible){
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


    public void NewToDeffend(int deffendAt, DeffendableSector defSect){
        if (this.toDeffend.ContainsKey(deffendAt)){
            List<DeffendableSector> tmp = this.toDeffend[deffendAt];
            tmp.Add(defSect);
            this.toDeffend[deffendAt] = tmp; 
        } else {
            this.toDeffend.Add(deffendAt, (new List<DeffendableSector>{defSect}));
        }
    }

    public void CheckSectors(int currentTime){
        bool passed; 
        if (this.toDeffend.ContainsKey(currentTime)){
            WarningMessage wM = FindObjectOfType<WarningMessage>();
            foreach(DeffendableSector dS in this.toDeffend[currentTime]){
                if (!this.selectedSectors.Contains(dS.sectorNum)){
                    wM.AddWarning(dS.sectorNum, dS.susPunish);
                    FindObjectOfType<susBar>().increaseSus(dS.susPunish);

                    passed = true;
                } else {  // if protected
                    wM.AddApproval(dS.sectorNum);
                    passed = false;
                }

                
                if(this.storyLines.ContainsKey(dS.storyNum)){
                    List<bool> tmp = this.storyLines[dS.storyNum];
                    tmp.Add(passed);
                    this.storyLines[dS.storyNum] = tmp;
                } else {
                    this.storyLines.Add(dS.storyNum, (new List<bool>{passed}));
                }
            }
        }
    }

    public Dictionary<int, List<bool>> getStoryLines(){
        return this.storyLines;
    }

    
}
