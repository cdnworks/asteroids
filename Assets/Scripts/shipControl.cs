using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    //global ship parameters
    [Header("Ship Attributes")]
    public float accRate = 30.0f;
    public float turnRate = 5.0f;
    public float maxSpeed = 10.0f;
    public float shieldTime = 3.0f;
    public float shieldTimeMax = 3.0f;


    //reference to bullet and shield prefabs
    public GameObject bullet;
    public GameObject shield;

    //local ship parameters
    float xInput = 0, yInput = 0;
    float rotationAng = 0;
    bool shieldUp = false;
    bool shieldOnCD = false;


    //components
    Rigidbody2D shipRigidBody2D;


    // Start is called before the first frame update
    void Start()
    {
        //fetch rigidbody2D component
        shipRigidBody2D = GetComponent<Rigidbody2D>();


    }

    // Since the ship is a RigidBody, use FixedUpdate to apply forces and such
    void FixedUpdate()
    {
        ApplyEngineForce();
        ApplySteeringForce();
        ShieldTimer(shieldUp);
        
    }

    void ApplyEngineForce()
    {
        //cap max speeds in forward, reverse and arbitrary directions respectively
        if (shipRigidBody2D.velocity.magnitude > maxSpeed && yInput > 0)
            return;

        if (shipRigidBody2D.velocity.magnitude < -maxSpeed * 0.5f && yInput < 0)
            return;

        if (shipRigidBody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && yInput > 0)
            return;


        //forward 'engine' vector
        Vector2 engineForceVector = transform.up * yInput * accRate;

        //apply vector as force
        shipRigidBody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteeringForce()
    {

        //get a rotation angle from input, scale by turn rate
        rotationAng -= xInput * turnRate;

        //apply this angle to the ship
        shipRigidBody2D.MoveRotation(rotationAng);

    }


    public void SetInputVector(Vector2 inputVector)
    {
        //collect input from shipInputHandler.cs
        xInput = inputVector.x;
        yInput = inputVector.y;
    }

    //fire the weapons and stuff
    public void FireWeapons(bool isFiring)
    {
        if(isFiring)
        {
            //fire the thing
            Instantiate(bullet, shipRigidBody2D.transform.position,shipRigidBody2D.transform.rotation);

        }
    }


    public void IsThrusting(out bool isThrusterOn)
    {
        isThrusterOn = false;
        //basically check if the player is applying thrust, if so, set a flag for fireFXHandler to read to spawn the fire effect
        if (yInput > 0)
        {
            isThrusterOn = true;
        }
        else return;

    }



    //Shield Handling, spawns shield centered on the player that follows the player.
    public void DeployShield(bool shieldPress,bool shieldRelease)
    {
        if (shieldPress && shieldOnCD == false)
        {
            //deploy the shield
            Instantiate(shield, shipRigidBody2D.transform.position, Quaternion.identity, gameObject.transform);
            shieldUp = true;


        }
        //destroy the shield on button release
        if (shieldRelease)
        {
            Destroy(GameObject.FindGameObjectWithTag("Shield"));
            shieldUp = false;
        }
    }





    public void ShieldTimer(bool isShieldUp)
    {
        if(isShieldUp)
        {
            shieldTime -= Time.deltaTime;
            Debug.Log(shieldTime + " Seconds of Shield Remaining");

            if (shieldTime <= 0)
            {
                //ran outta shields! Break the shield and get out of the function to prevent unwanted timer drain
                shieldOnCD = true;
                Destroy(GameObject.FindGameObjectWithTag("Shield"));
                Debug.Log("Shields on Cooldown!");
                return;

            }
        }
        if(isShieldUp == false && shieldTime <= shieldTimeMax)
        {
            shieldTime += Time.deltaTime / 2;
            Debug.Log("Shields Recharging with " + shieldTime + " Seconds Available");

            if(shieldOnCD && shieldTime >= shieldTimeMax)
            {
                shieldOnCD = false;
                Debug.Log("Shield Cooldown Over!");
            }
        }

    }


}

