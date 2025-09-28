using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnManager;
    public float spawnInterval = 3f;
    private float timer;
    private int currentSpawnIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        timer = spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnEnemy();
            timer = spawnInterval;
        }
    }

    public void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnManager.position, spawnManager.rotation);
    }
}
