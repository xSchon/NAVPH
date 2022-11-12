using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject brick;
    private GameObject active_brick;
    private float score = 0f;
    // Start is called before the first frame update
    void Start()
    {
        spawNewBrick();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)){
            spawNewBrick();
        }
    }

    public void spawNewBrick(){
        active_brick = Instantiate(brick,
                        transform.position,
                        Quaternion.identity);
    }

    public void incremenmtScore(float height){
        if (height > score) score = height;
        Debug.Log(score);
    }
}
