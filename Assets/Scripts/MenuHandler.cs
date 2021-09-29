using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuHandler : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("Game");
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("Menu");
    }



}
