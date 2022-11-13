using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBehavior : MonoBehaviour
{
    private bool has_gravity = false;
    public bool hasEntered = false;
    private bool movingLeft = true;
    public float speed = 0.03f;
    public float mass = 2f;
    private Minigame1 spawner;
    public Rigidbody rb;

    void Awake() {
        rb = GetComponent<Rigidbody>();
     }

    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<Minigame1>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
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

    void FixedUpdate()          // stops bouncing completly
    {
        var currentVelocity = rb.velocity;

        if (currentVelocity.y <= 0f) 
            return;
        
        currentVelocity.y = 0f;
        
        rb.velocity = currentVelocity;
     }

    void addGravity(){
        rb.useGravity = true;
        has_gravity = true;
    }

    void OnCollisionEnter (Collision other) {
        if (other.gameObject && !hasEntered) {
            hasEntered = true;

            spawner.incremenmtScore(transform.position.y);
            spawner.spawNewBrick();
        }
   }
}
