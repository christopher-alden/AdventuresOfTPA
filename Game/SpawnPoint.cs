using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    private ArenaManager arenaManager;
    [SerializeField] private GameObject playerAttackerPrefab;
    [SerializeField] private GameObject towerAttackerPrefab;
    private bool lastSpawnWasPlayerAttacker = false;
    private int enemyCount;
    private int playerAttackerCount;
    private int towerAttackerCount;
    private float spawnInterval = 10f;
    private float timer = 0f;


    private void CheckEnemyCount()
    {
        List<EnemyManager> enemies = arenaManager.EnemyList;
        enemyCount = enemies.Count;
        playerAttackerCount = arenaManager.PlayerAttacker;
        towerAttackerCount = arenaManager.TowerAttacker;
        Debug.Log("player:"+playerAttackerCount + "tower:"+towerAttackerCount + "total:"+enemyCount);
    }

    private void SpawnEnemy()
    {
        if (enemyCount < 6)
        {
            GameObject prefabToInstantiate = lastSpawnWasPlayerAttacker ? towerAttackerPrefab : playerAttackerPrefab;
            
            if ((lastSpawnWasPlayerAttacker && towerAttackerCount < 2) || (!lastSpawnWasPlayerAttacker && playerAttackerCount < 3))
            {
                GameObject instantiatedPrefab = Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);
                EnemyManager enemy = instantiatedPrefab.GetComponent<EnemyManager>();
                arenaManager.AddEnemy(enemy);
                timer = 0f;
                lastSpawnWasPlayerAttacker = !lastSpawnWasPlayerAttacker;
            }
        }
    }

    private void Start()
    {
        arenaManager = ArenaManager.Instance;
        SpawnEnemy();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        CheckEnemyCount();
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
        }
    }
}
