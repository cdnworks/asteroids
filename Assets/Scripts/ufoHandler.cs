using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ufoHandler : MonoBehaviour
{
    //global vars
    [Header("UFO Atributes")]
    public float minSpeed = 1.0f;
    public float maxSpeed = 5.0f;
    public float firingInterval = 2.0f;


    //reference to UFO bullet prefab
    public GameObject ufoBullet;

    //local vars
    float activeDuration = 0f;
    float speedChangeTime;
    float refireTime;
    Vector2 changedSpeed = Vector2.zero;

    //components
    Rigidbody2D ufoRigidBody;



    // Start is called before the first frame update
    void Start()
    {
        ufoRigidBody = GetComponent<Rigidbody2D>();
        SpawnUFO();
    }

    // Update is called once per frame
    void Update()
    {
        SpeedControl();
        FireControl();
        DespawnUFO();
        
    }



    public void SpawnUFO()
    {
        //determine transform direction
        int dirScalar = Random.Range(0, 2);
        if (dirScalar == 0)
            dirScalar = -1; //dumb workaround, figure out how to nicely select either -1 or 1

        //determine initial speed and set it
        float initialSpeed = Random.Range(minSpeed, maxSpeed);
        ufoRigidBody.velocity = new Vector2(dirScalar * initialSpeed, 0);


        //determine initial position
        float yPosition = Random.Range(4, -4); //without knowing the exact screen space available, this is an estimate from the camera space within the unity sceneviewer. Presumably, in Unity Units.
        ufoRigidBody.transform.position = new Vector2(10 * dirScalar, yPosition);


        //generate the ufo duration timer and the speed change interval and new speed, copy firing interval for use in FireControl()
        activeDuration = Random.Range(10, 20);
        speedChangeTime = Random.Range(4, 8);
        refireTime = firingInterval;

        changedSpeed.x = ufoRigidBody.velocity.x + ufoRigidBody.velocity.x * Random.Range(-0.2f, 0.2f);
        //just in case the range pulls a 0, again, dumb workaround but whatever I dont wanna make a list or something 
        if (changedSpeed.x == 0)
            changedSpeed.x = initialSpeed + 1;

        

    }


    public void DespawnUFO()
    {
        activeDuration -= Time.deltaTime;
        if (activeDuration <= 0)
        {
            //duration elapsed, the ufo disappears when it goes off screen!
            if (gameObject.transform.position.x >=10 || gameObject.transform.position.x <= -10)
            {
                Destroy(gameObject);
            }

        }


    }

    public void SpeedControl()
    {
        speedChangeTime -= Time.deltaTime;
        if (speedChangeTime <= 0)
        {
            ufoRigidBody.velocity = changedSpeed;
        }
    }

    public void FireControl()
    {
        //shoot things at the player after an interval.
        firingInterval -= Time.deltaTime;
        if (firingInterval <= 0)
        {
            //find the player and calculate the vector from the UFO to the player
            Vector2 shipPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            Vector2 shotVector = new Vector2(gameObject.transform.position.x - shipPosition.x, gameObject.transform.position.y - shipPosition.y);
            //make a quaternion
            Quaternion shotRotation = Quaternion.LookRotation(Vector3.forward,shotVector);
            


            Instantiate(ufoBullet, ufoRigidBody.transform.position, shotRotation); //the third arg needs to be the angle the player is relative to the UFO.
            firingInterval = refireTime; // resets timer for firing
        }
    }



    //collider handling

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //kill the player, remove a life, trigger reset player position etc.
            Debug.Log("The UFO hit the Player!");
        }

        if (collision.CompareTag("Bullet") || collision.CompareTag("Shield"))
        {
            //Destroy the UFO
            Destroy(gameObject);

        }
    }



}



