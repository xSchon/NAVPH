/* Script, that manages behavior of each brick in minigame-1. */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBehavior : MonoBehaviour
{
    private bool has_gravity = false;
    public bool hasEntered = false;
    public bool tutorial = true;
    private Vector3 dir = Vector3.left;
    private float speed;
    public float mass = 2f;
    private Minigame1 spawner;
    public Rigidbody rb;

    void Start()
    {
        spawner = FindObjectOfType<Minigame1>();

        int difficulty = PlayerPrefs.GetInt("difficulty", 2);
        if (difficulty == 1) { speed = 10f; } // easy
        if (difficulty == 2) { speed = 20f; } // normal
        if (difficulty == 3) { speed = 20f; } // hard
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            addGravity();
        }

        // Change direction
        if (!has_gravity)
        {
            if (transform.position.x <= -5)
            {
                dir = Vector3.right;
            }
            else if (transform.position.x >= 5)
            {
                dir = Vector3.left;
            }

            transform.Translate(dir * speed * Time.deltaTime);
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
