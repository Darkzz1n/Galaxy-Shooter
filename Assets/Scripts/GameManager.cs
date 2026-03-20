using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;
    
    public bool _isCoopMode;

    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.buildIndex == 2)
        {
            _isCoopMode = true;
            Debug.Log("Coop Mode");
        }
        else if (currentScene.buildIndex == 1)
        {
            _isCoopMode = false;
            Debug.Log("Single Player");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1); //Current Game Scene
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Quitted Game");
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}


