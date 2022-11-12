using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBehavior : MonoBehaviour
{
    private bool has_gravity = false;
    public bool hasEntered = false;
    private bool movingLeft = true;
    public float speed = 0.03f;
    private Spawner spawner;
    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)){
            addGravity();
        }

        if (!has_gravity){
            if (movingLeft){
                transform.position += new Vector3(-speed,0,0);

                if (transform.position.x < -5) movingLeft = false;
            }
            else{
                transform.position += new Vector3(speed,0,0);

                if (transform.position.x > 5) movingLeft = true;
            }
        }
    }

    void addGravity(){
        Rigidbody gameObjectsRigidBody = gameObject.AddComponent<Rigidbody>();
        has_gravity = true;
    }

    void OnCollisionEnter (Collision other) {
        if (other.gameObject && !hasEntered) {
            hasEntered = true;

            spawner.spawNewBrick();
            spawner.incremenmtScore(transform.position.y);
        }
   }
}
