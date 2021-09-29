using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldBoundaryHandler : MonoBehaviour
{
    // this script should manage when the player or enemy objects hit the edges of the play space,
    // and move them to an appropriate position instead of letting them fly offscreen

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        //can just modify this position without the method, just leaving it for right now
        other.gameObject.transform.position = ScreenWrapTransform(other.gameObject.transform.position);

    }

    //this gets the gameObject's position when colliding with the boundary,
    //and calculates where it should spawn on the other edge of the screen
    private Vector2 ScreenWrapTransform(Vector2 oldPosition)
    {
        Vector2 newPosition = oldPosition*-1;
        return newPosition;
    }
}
