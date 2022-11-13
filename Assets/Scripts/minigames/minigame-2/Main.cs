using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    static public Main Instance;

    private int count = 0;
    private void Awake() 
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore()
    {
        count++;
        Debug.Log(count);
        if (count == 4)
        {
            Debug.Log("You Win!");
        }

    }
}
