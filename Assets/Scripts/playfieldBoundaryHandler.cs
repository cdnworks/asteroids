using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playfieldBoundaryHandler : MonoBehaviour
{
    // this script should manage when the player or enemy objects hit the edges of the play space,
    // and move them to an appropriate position instead of letting them fly offscreen
    GameObject player;
    GameObject asteroid;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        asteroid = GameObject.FindGameObjectWithTag("Asteroid");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            Debug.Log("The player is at: " + player.transform.position);
            player.transform.position = ScreenWrapTransform(player.transform.position);
            Debug.Log("The player went to: " + player.transform.position);
        }

        if (collision.CompareTag("Asteroid"))
        {
            Debug.Log("The Asteroid is at: " + asteroid.transform.position);
            asteroid.transform.position = ScreenWrapTransform(asteroid.transform.position);
            Debug.Log("The Asteroid went to: " + asteroid.transform.position);
        }


    }

    //this gets the gameObject's position when colliding with the boundary,
    //and calculates where it should spawn on the other edge of the screen
    private Vector2 ScreenWrapTransform(Vector2 oldPosition)
    {
        Vector2 newPosition = oldPosition*-1;
        return newPosition;
    }
}
