using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player_1;

    private Player _player_2;

    private Animator _animator;

    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    private float _canFire = -1f;
    [SerializeField]
    private float _fireRate = -3f;

    [SerializeField]
    private AudioClip _enemyLaserSoundClip;

    [SerializeField]
    private AudioClip _destroySoundClip;
    [SerializeField]
    private AudioSource _audioSource;

    void Start()
    {
        _player_1 = GameObject.Find("Player_1").GetComponent<Player>();
        if (_player_1 == null)
        {
            Debug.LogError("Player 1 is NULL");
        }

        _player_2 = GameObject.Find("Player_2").GetComponent<Player>();
        if (_player_2 == null)
        {
            Debug.LogError("Player 2 is NULL");
        }

        _animator = GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("Animator is NULL");
        }

        _audioSource = GetComponent<AudioSource>();

        StartCoroutine(SpawnLaserRoutine());
    }

    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(randomX, 8, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player_1 != null)
            {
                _player_1.AddScore(10);
            }
            if (_player_2 != null)
            {
                _player_2.AddScore(10);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.clip = _destroySoundClip;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.clip = _destroySoundClip;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }

    IEnumerator SpawnLaserRoutine()
    {
        _audioSource.clip = _enemyLaserSoundClip;
        yield return new WaitForSeconds(Random.Range(0.1f, 1.0f));
        Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
        _audioSource.Play();
    }
}
