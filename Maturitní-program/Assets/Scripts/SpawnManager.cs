using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject warriorPrefab; 
    public GameObject archerPrefab;  
    public Transform spawnPoint;     

    private Queue<GameObject> spawnQueue = new Queue<GameObject>(); 
    public float spawnInterval = 2f; 
    private bool isSpawning = false; 

    public void SpawnWarrior()
    {
        if(GameManager.Instance.SubtractGold(20))
        {
            spawnQueue.Enqueue(warriorPrefab); 
            TrySpawnUnit();

        }
    }

    public void SpawnArcher()
    {
        if (GameManager.Instance.SubtractGold(20))
        {
            spawnQueue.Enqueue(archerPrefab);
            TrySpawnUnit();
        }
    }

    private void TrySpawnUnit()
    {
        if (!isSpawning && spawnQueue.Count > 0) // Spustí spawnování, pokud neběží
        {
            StartCoroutine(SpawnUnit());
        }
    }

    private IEnumerator SpawnUnit()
    {
        isSpawning = true; // Zamezí souběžnému spawnu

        while (spawnQueue.Count > 0)
        {
            GameObject unitToSpawn = spawnQueue.Dequeue(); // Vezme první jednotku z fronty
            Instantiate(unitToSpawn, spawnPoint.position, Quaternion.identity); // Spawn jednotky
            yield return new WaitForSeconds(spawnInterval); // Počkej 1 sekundu před dalším spawnem
        }

        isSpawning = false; // Po dokončení vypne kontrolu spawnu
    }
}
