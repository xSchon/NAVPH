using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveArmy : MonoBehaviour
{
    // private GameObject sectors;
    // Start is called before the first frame update
    void Start()
    {
        // sectors = GameObject.Find("sectorsDeffence").GetComponent<TextMeshProUGUI>();
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
