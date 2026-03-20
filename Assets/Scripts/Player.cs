using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    public bool _isPlayerOne;
    public bool _isPlayerTwo;

    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private float _speedMultiplier = 2;


    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private GameObject _leftEngine, _rightEngine;

    [SerializeField]
    private float _fireRate = 0.075f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    private GameManager _gameManager;

    [SerializeField]
    private bool _isTripleShotActivated = false;
    
    [SerializeField]
    private bool _isShieldActivated = false;

    [SerializeField]
    private int _score;

    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioSource _audioSource;

    private UiManager _uiManager;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();

        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source is NULL");
        } else
        {
            _audioSource.clip = _laserSoundClip;
        }

        if (_gameManager._isCoopMode == false)
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }

    void Update()
    {
        if (_isPlayerOne == true)
        {
            PlayerOneMovement();

          if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && Time.time > _canFire && _isPlayerOne == true)
        {
            PlayerOneFireLaser();
        }
        }

        if (_isPlayerTwo == true)
        {
            PlayerTWoMovement();

          if ((Input.GetKeyDown(KeyCode.Shift) || Input.GetMouseButtonDown(0)) && Time.time > _canFire && _isPlayerTwo == true);
        }
    }
    void PlayerOneMovement()
    {
        float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal"); // Input.GetAxis("Horizontal");
        float verticalInput = CrossPlatformInputManager.GetAxis("Vertical"); // Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0); ;
        }
    }
    void PlayerTwoMovement()
    {
        float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal"); // Input.GetAxis("Horizontal");
        float verticalInput = CrossPlatformInputManager.GetAxis("Vertical"); // Input.GetAxis("Vertical");

        if (verticalInput.GetKey(KeyCode.J))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
         if (verticalInput.GetKey(KeyCode.L))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
         if (verticalInput.GetKey(KeyCode.I))
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
         if (verticalInput.GetKey(KeyCode.K))
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0); ;
        }
    }
    void PlayerOneFireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActivated == true)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSource.Play();
    }
    void PlayerTwoFireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActivated == true)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldActivated == true)
        {
            _isShieldActivated = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Game");
            }
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy_Laser")
        {
            Damage();
            Destroy(other.gameObject);
        }
    }
    public void TripleShotActive()
    {
        _isTripleShotActivated = true;
        Debug.Log("Triple Shot Activated");
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedBoostActivate()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
        Debug.Log("Speed Boost Activated");
    }

    public void ShieldActivate()
    {
        _isShieldActivated = true;
        _shieldVisualizer.SetActive(true);
        Debug.Log("Shield Activated");
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _isTripleShotActivated = false;
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
