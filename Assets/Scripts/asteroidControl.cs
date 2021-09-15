using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroidControl : MonoBehaviour
{
    //global asteroid perameters
    public float minSpeed = 0.1f, maxSpeed = 5.0f;


    //local asteroid perameters
    float trajectory, speed;
    Vector2 velocityVector;


    //components 
    Rigidbody2D asteroidRigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        asteroidRigidBody2D = GetComponent<Rigidbody2D>();
        trajectory = GetInitialTrajectory();
        speed = GetInitialSpeed();
        velocityVector = MakeVector(trajectory, speed);
        asteroidRigidBody2D.velocity = velocityVector;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //generate a random trajectory for the asteroid on asteroid creation
    private float GetInitialTrajectory()
    {
        return Random.Range(0, 360);
    }

    //generate a random speed for the asteroid
    private float GetInitialSpeed()
    {
        return Random.Range(minSpeed, maxSpeed);
    }

    //generate vector
    private Vector2 MakeVector(float angle, float magnitude)
    {
        //generate vector componets
        float xComp, yComp;
        xComp = Mathf.Cos(angle) * magnitude;
        yComp = Mathf.Sin(angle) * magnitude;

        Vector2 velocityVector = new Vector2(xComp, yComp);
        return velocityVector;
    }

    //collider handling
    //the asteroid can collide with a few things in the scene:
    // the ship, player gun shots and other asteroids. However, the play area is also a collider
    //we must ignore the playspace collider when defining collision behaviors.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //kill the player, remove a life, trigger reset player position etc.
            Debug.Log("The Astroid hit the Player!");
        }
    }
}
