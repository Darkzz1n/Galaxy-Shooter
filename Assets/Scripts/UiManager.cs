using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _playerOneLivesImg;
    [SerializeField]
    private Image _playerTwoLivesImg;
    [SerializeField]
    private Sprite[] _playerOneLiveSprites;
    [SerializeField]
    private Sprite[] _playerTwoLiveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;

    private GameManager _gameManager;

    private int _playersAlive;



    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager está NULL");
        }

        _playersAlive = GameObject.FindGameObjectsWithTag("Player").Length;
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int playerID, int currentLives)
    {
        if (playerID == 1 && _playerOneLivesImg != null)
        {
            _playerOneLivesImg.sprite = _playerOneLiveSprites[currentLives];
        }
        else if (playerID == 2 && _playerTwoLivesImg != null)
        {
            _playerTwoLivesImg.sprite = _playerTwoLiveSprites[currentLives];
        }
        
    }

    public void NotifyPlayerDeath()
    {
        _playersAlive--;

        if (_playersAlive <= 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();

        GameObject spawnManager = GameObject.Find("Spawn_Manager");
        if (spawnManager != null)
        {
            spawnManager.GetComponent<SpawnManager>().OnPlayerDeath();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Game");
        }

        StartCoroutine(GameOverFlickerRoutine());
        _restartText.gameObject.SetActive(true);
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
