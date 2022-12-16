/* Script, that manages behavior of each brick in minigame-1. */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBehavior : MonoBehaviour
{
    private bool has_gravity = false;
    public bool hasEntered = false;
    public bool tutorial = true;
    private bool movingLeft = true;
    public float speed = 0.03f;
    public float mass = 2f;
    private Minigame1 spawner;
    public Rigidbody rb;

    void Start()
    {
        spawner = FindObjectOfType<Minigame1>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            addGravity();
        }

        if (!has_gravity)
        {
            if (movingLeft)
            {
                transform.position += new Vector3(-speed, 0, 0);

                if (transform.position.x < -5) movingLeft = false;
            }
            else
            {
                transform.position += new Vector3(speed, 0, 0);

                if (transform.position.x > 5) movingLeft = true;
            }
        }
    }

    // Stops bouncing completly
    void FixedUpdate()
    {
        var currentVelocity = rb.velocity;

        if (currentVelocity.y <= 0f)
            return;

        currentVelocity.y = 0f;

        rb.velocity = currentVelocity;
    }

    // Add gravity (rigidbody) to this brick
    void addGravity()
    {
        rb.useGravity = true;
        has_gravity = true;
    }

    // When brick falss down, spawn new and stop interacting with others.
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject && !hasEntered)
        {
            hasEntered = true;

            spawner.AddPlacedBrick(this.gameObject);
            spawner.SpawNewBrick();
        }
    }
}
