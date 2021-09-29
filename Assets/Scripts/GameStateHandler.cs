using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameStateHandler : MonoBehaviour
{
    //object references
    public TMP_Text pointsText;
    public TMP_Text livesText;

    public GameObject player;
    public GameObject ufo;
    public GameObject bigAsteroid;
    public GameObject medAsteroid;
    public GameObject smlAsteroid;

    [Header("Game Parameters")]
    public int playerLives = 4;
    public float transitionTime = 3.0f;


    int gameLevel;
    float ufoSpawnTimer;
    float transitionTimeReset;

    [HideInInspector]
    public static int numBigAst, numMedAst, numSmlAst, numUFO, score;
    public static bool isPlayerAlive;




    // Start is called before the first frame update
    void Start()
    {
        gameLevel = 1;
        score = 0;
        ResetStage();
    }

    // Update is called once per frame
    void Update()
    {
        pointsText.text = "" + score;
        livesText.text = "" + playerLives;
        IsPlayerAlive(isPlayerAlive);
        SpawnUFO();
        AdvanceStage();
    }

    //what should this script control and manage?
    //
    //Gameplay/Enemies:
    //track what 'level' the player is on
    //generate and track an arbitrary number of asteroids (only initially generating large asteroids based on current 'level')
    //whenever an astroid is destroyed, remove it from the tracker, grant points based on the type of asteroid destroyed
    //generate ufos based on level. Maybe like, every 4th level or something you get a UFO maybe more if it's a large multiple of the arbitrary level.
    //define a spawn bubble around the player where asteroids or UFOs cannot spawn (maybe spawn everything off screen so it'll warp in from the edges)
    //check if there are no more asteroids and/or UFOs in the scene, if so, the level is 'completed'
    //if the level is completed, wait some time, then spawn a new set of asteroids. Increment the 'level'
    //repeat
    //
    //Player:
    //Track the player. Meaning lives. Set lives to some arbitrary value, update the text.
    //check if the player has been 'destroyed' since the the scene began.
    //if the player is destroyed, clear the scene out, pause for drama (and to let the player know they died) respawn the player, respawn asteroids based on the level
    //decrement a life if the player is destroyed.
    //monitor the points value, and every X number of points grants an extra life. 
    //if the player runs out of lives, pause for drama, boot them out to the game over scene.




    //this is called on the start of a new game, or when the player dies.
    private void ResetStage()
    {
        numMedAst = 0;
        numSmlAst = 0;
        isPlayerAlive = true;
        transitionTimeReset = transitionTime;

        //clear out any pre-existing prefab objects
        var asteroidPrefabs = GameObject.FindGameObjectsWithTag("Asteroid");
        foreach (var clone in asteroidPrefabs)
        {
            Destroy(clone);
        }


        var ufoPrefabs = GameObject.FindGameObjectsWithTag("UFO");
        foreach (var clone in ufoPrefabs)
        {
            Destroy(clone);
        }


        //pause before spawning things

        //spawn player
        Instantiate(player, Vector3.zero, Quaternion.identity);

        //spawn and initialize tracking for asteroids
        GenerateAsteroids(gameLevel);

        //initialize number of UFOs and the spawn timers
        InitializeUFO(gameLevel);
    }

    //this is called when the player destroys all asteroids and UFOs
    private void AdvanceStage() 
    {
        if(numBigAst == 0 && numMedAst == 0 && numSmlAst == 0 && numUFO == 0)
        {
            //pause between levels
            transitionTime -= Time.deltaTime;
            if(transitionTime <= 0)
            {
                gameLevel += 1;

                //pause before spawning things


                //spawn and initialize tracking for asteroids
                GenerateAsteroids(gameLevel);

                //initialize number of UFOs and the spawn timers
                InitializeUFO(gameLevel);

                //reset pause timer
                transitionTime = transitionTimeReset;
            }

        }

    }


    //generate the number of astroids in the current level, Asteroids posess their own positioning routine, so instantiate them at the origin.
    private void GenerateAsteroids(int gameLevel)
    {
        numBigAst = 2 + gameLevel;

        for (int i = 0; i < numBigAst; i++)
            Instantiate(bigAsteroid, Vector3.zero, Quaternion.identity);

    }



    //determine if the level will have a UFO, set the number of UFOs, and set a timer for when the UFO(s) will spawn into the level
    private void InitializeUFO(int gameLevel)
    {
        if (gameLevel % 4 == 0)
        {
            numUFO = Random.Range(0, 2);
        }

        
        //make spawn timer
        if (numUFO != 0)
        {
            ufoSpawnTimer = Random.Range(5, 10);
        }
    }

    //spawn in the UFO, which has it's own position randomization in it's script, so just throw it in the origin
    private void SpawnUFO()
    {
        ufoSpawnTimer -= Time.deltaTime;
        if (ufoSpawnTimer <= 0)
        {
            for (int i = 0; i < numUFO; i++)
                Instantiate(ufo, Vector3.zero, Quaternion.identity);
        }

    }

    private void IsPlayerAlive(bool isPlayerAlive)
    {
        //does nothing unless the player dies.

        if(!isPlayerAlive)
        {
            //this flag is tripped by collision handling in the asteroids' and ufo's scripts
            //here we destroy the player, decrement lives and reset the scene at the current level if they get hit
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            playerLives -= 1;


            //the gameover state is handled here.
            if (playerLives == 0) //<= used in case of rapid collision with something while waiting, a dumb fix but it works
            {
                SceneManager.LoadScene("EndScreen");
            }
            else
                ResetStage();
            

        }




    }


}
