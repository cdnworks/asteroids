using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipControl : MonoBehaviour
{
    //global ship parameters
    [Header("Ship Attributes")]
    public float accRate = 30.0f;
    public float turnRate = 5.0f;
    public float maxSpeed = 10.0f;

    //local ship parameters
    float xInput = 0, yInput = 0;
    float rotationAng = 0;

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


}

