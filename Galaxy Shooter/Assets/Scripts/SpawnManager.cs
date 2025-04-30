using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator SpawnRoutine()
    {
        while(1 > 0)
        {
            Instantiate(_enemyPrefab, transform.position + new Vector3(0, 6, 0), Quaternion.identity);
            yield return new WaitForSeconds(5);

        }
    }
}
