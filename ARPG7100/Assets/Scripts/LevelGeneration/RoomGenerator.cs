using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
    {
    public List<GameObject> spawnPoints;
    public List<GameObject> enemyPrefabs;
    public float maxEnemyPacks;
    public float maxEnemies;

    float totalEnemies;
    List<GameObject> toDestroy = new List<GameObject>();
    private void Start()
        {
        for (int i = 0; i < maxEnemyPacks; i++)
            {
            //if we already have enough enemies in the dungeon tile, end the loop
            if (totalEnemies >= maxEnemies)
                break;

            //pick the spawn point and type of enemypack from the lists
            int chosenSpawn = Random.Range(0, spawnPoints.Count);
            int chosenPack = Random.Range(0, enemyPrefabs.Count);
            GameObject spawnPoint = spawnPoints[chosenSpawn];
            GameObject enemyPack = enemyPrefabs[chosenPack];

            //spawn the enemy pack
            Instantiate(enemyPack, spawnPoint.transform.position, Quaternion.identity);

            //check how many enemies are in the pack and add it to the totalenemies
            totalEnemies += enemyPack.GetComponentsInChildren<EnemyController>().Length;

            //remove the used spawn point from the list of potential spawns
            toDestroy.Add(spawnPoints[chosenSpawn]);
            spawnPoints.Remove(spawnPoints[chosenSpawn]);
            }

        //once were done spawning enemies, destroy all the irrelevant objects
        foreach (GameObject obj in spawnPoints)
            Destroy(obj);
        foreach (GameObject obj in toDestroy)
            Destroy(obj);
        Destroy(gameObject);

        }
    }
