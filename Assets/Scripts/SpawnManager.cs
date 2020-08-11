using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]  private GameObject _enemyPrefab;
    [SerializeField]  private GameObject _enemyContainer; 
    // Container - If player misses it, it will repawn
    
    [SerializeField]     private GameObject[] powerups;
    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        // method #2 
        //StartCoroutine("SpawnRoutine");
    }

    // Spawn game objects every 5 seconds
    //Create a coroutine of type IEnumerator -- Yield Events
    //while loop

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);
        //while loop (Infinite loop as it is in routine)
        //Instantiate enemy prefab
        //yield wait for 5 seconds

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            
            // these 2 lines will organise and enemy will become child of spawn manager

            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
             
            
            yield return new WaitForSeconds(5.0f);
        }
        // * yield return null; // wait for 1 frame
                //then this line is called
         ///* yield return new WaitForSeconds(5.0f);
                //then this line is called
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);

        // Every 3-7 seconds, spawn in a powerup
        while (_stopSpawning==false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(powerups[randomPowerUp], posToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3, 8)); 
            // 3,4,5,6,7 as it is int

        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
