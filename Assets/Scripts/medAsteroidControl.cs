using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedAsteroidControl : MonoBehaviour
{
    //global asteroid perameters
    public float minSpeed = 0.1f, maxSpeed = 5.0f;
    public int medScoreValue = 100;
    public GameObject smlAsteroid;


    //local asteroid perameters
    float trajectory, speed, rotationFactor;
    Vector2 velocityVector;


    //components 
    Rigidbody2D asteroidRigidBody2D;


    // Start is called before the first frame update
    void Awake()
    {
        asteroidRigidBody2D = GetComponent<Rigidbody2D>();
        trajectory = GetInitialTrajectory();
        speed = GetInitialSpeed();
        velocityVector = MakeVector(trajectory, speed);
        rotationFactor = Random.Range(-speed / 10, speed / 10);    //create a random factor to rotate the asteroid by based on it's speed, looks cooler.
        asteroidRigidBody2D.velocity = velocityVector;
        GameStateHandler.numMedAst += 1; //increment the number of medium asteroids in the gamestate handler by 1
        
    }

    // Update is called once per frame
    void Update()
    {
        asteroidRigidBody2D.transform.Rotate(Vector3.forward * rotationFactor);
        DestroyIfOutOfBounds();
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

    //out of bounds correction
    //This is a bug fix, in case the asteroid esdcapes the playfield boundary
    private void DestroyIfOutOfBounds()
    {
        if (gameObject.transform.position.x > 20 || gameObject.transform.position.x < -20 || gameObject.transform.position.y > 20 || gameObject.transform.position.y < -20)
        {
            Destroy(gameObject);
        }
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
            GameStateHandler.isPlayerAlive = false;
        }

        if (collision.CompareTag("Bullet") || collision.CompareTag("Shield"))
        {
            //spawn one to four small asteroids, destroy the original asteroid
            int range = Random.Range(1, 4);
            for (int i = 0; i < range; i++)
            {
                Instantiate(smlAsteroid, asteroidRigidBody2D.transform.position, transform.rotation);
            }
            //decrement the number of medium asteroids, add score in gameStateHandler(); and destroy the asteroid
            GameStateHandler.numMedAst -= 1;
            GameStateHandler.score += medScoreValue;
            Destroy(gameObject);

        }
    }
}
