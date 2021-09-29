using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //components
    //this script will use info from the shipControl script
    ShipControl shipControl;


    private void Awake()
    {
        shipControl = GetComponent<ShipControl>();
    }

    // Update is called once per frame
    void Update()
    {
        //collect user input
        Vector2 inputVector = Vector2.zero;

        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");
        shipControl.SetInputVector(inputVector);

        if (Input.GetKeyDown("space"))
        {
            shipControl.FireWeapons(true);
        }

        if (Input.GetKeyDown("left shift"))
        {
            shipControl.DeployShield(true,false);
        }
        if (Input.GetKeyUp("left shift"))
        {
            shipControl.DeployShield(false, true);
        }
    }
}
