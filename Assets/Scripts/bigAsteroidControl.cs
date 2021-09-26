using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bigAsteroidControl : MonoBehaviour
{
    //global asteroid perameters
    public float minSpeed = 0.1f, maxSpeed = 5.0f;
    public GameObject medAsteroid;


    //local asteroid perameters
    float trajectory, speed, rotationFactor;
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
        rotationFactor = Random.Range(-speed / 10, speed / 10);    //create a random factor to rotate the asteroid by based on it's speed, looks cooler.
        asteroidRigidBody2D.velocity = velocityVector;
    }

    // Update is called once per frame
    void Update()
    {
        asteroidRigidBody2D.transform.Rotate(Vector3.forward * rotationFactor);
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
    // the ship, ship shields, player gun shots and other asteroids. However, the play area is also a collider
    //we must ignore the playspace collider when defining collision behaviors.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //kill the player, remove a life, trigger reset player position etc.
            Debug.Log("The Astroid hit the Player!");
        }

        if (collision.CompareTag("Bullet") || collision.CompareTag("Shield"))
        {
            //spawn one to three medium asteroids, destroy the original asteroid
            int range = Random.Range(1, 3);
            for (int i = 0; i < range; i++)
            {
                Instantiate(medAsteroid, asteroidRigidBody2D.transform.position, transform.rotation);
            }
            Destroy(gameObject);

        }
    }
}
