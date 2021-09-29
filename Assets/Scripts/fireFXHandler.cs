using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFXHandler : MonoBehaviour
{
    //local variables
    bool isThrusting;


    //components
    ShipControl shipControl;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        shipControl = GetComponentInParent<ShipControl>();
        spriteRenderer = GetComponent<SpriteRenderer>();


    }

    // Update is called once per frame
    void Update()
    {
        //get ship thrust status
        shipControl.IsThrusting(out isThrusting);

        if (isThrusting)
        {
            spriteRenderer.enabled = true;
        }
        else
            spriteRenderer.enabled = false;
 
    }
}
