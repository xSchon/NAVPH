using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;    

public class sectorsDeffence : MonoBehaviour
{   
    public int sectorsAmount; // set in Unity
    private int[] selectedSectors;
    private GameLogic gameLogic;
    // Start is called before the first frame update

    /*class Message
    {
        public int Radio
    }   
    Dictionary<string, Dictionary<string, Message>> data; */

    
    void Start()
    {
        if (sectorsAmount < 0 || sectorsAmount > 9){
            // if not set properly in unity, protect program from breaking
            sectorsAmount = 1; 
            }
        selectedSectors = new int[sectorsAmount];
        
        for(int i = 0; i < selectedSectors.Length; i += 1){
            selectedSectors[i] = 0;
        }

        gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        gameLogic.setSectors(selectedSectors);
    }

    // Update is called once per frame
    void Update()
    {
        
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

        gameLogic.setSectors(selectedSectors);
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
}
