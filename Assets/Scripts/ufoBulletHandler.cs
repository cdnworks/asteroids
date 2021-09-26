using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ufoBulletHandler : MonoBehaviour
{
    //global parameters
    public float shotSpeed = 5.0f;
    public float shotTime = 3.0f;
    //local parameters

    //components
    Rigidbody2D ufoBullet;


    void Awake()
    {
        //fetch rigid body component
        ufoBullet = GetComponent<Rigidbody2D>();
        //the bullet should just fly forward, and destroy itself after an elapsed time

        ufoBullet.velocity = transform.up * shotSpeed;
        Destroy(gameObject, shotTime);
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //destroy the bullet
            Debug.Log("The Shot Hit The Player!");
            Destroy(gameObject);
        }
    }
}
