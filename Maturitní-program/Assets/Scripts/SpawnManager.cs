using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int warriorCost = 20;
    public int wizzCost = 30;

    public GameObject warriorPrefab;  // Prefab pro Warriora
    public GameObject archerPrefab;   // Prefab pro Archera

    public Transform warriorSpawnPoint; // Místo spawnování Warriora
    public Transform archerSpawnPoint;  // Místo spawnování Archera

    private Queue<GameObject> warriorQueue = new Queue<GameObject>(); // Fronta pro Warriory
    private Queue<GameObject> archerQueue = new Queue<GameObject>();  // Fronta pro Archery

    

    public float spawnInterval = 10f; // Interval mezi spawny

    private bool isSpawningWarrior = false; // Kontrola spawnování Warriora
    private bool isSpawningArcher = false;  // Kontrola spawnování Archera

    
    
    public void SpawnWarrior()
    {
        if (GameManager.Instance.SubtractGold(warriorCost))
        {
            warriorQueue.Enqueue(warriorPrefab); // Přidá Warriora do fronty
            TrySpawnWarrior();
        }
        
    }

    public void SpawnArcher()
    {
        if (GameManager.Instance.SubtractGold(wizzCost))
        {
            archerQueue.Enqueue(archerPrefab); // Přidá Archera do fronty
            TrySpawnArcher();
        }
    }

    private void TrySpawnWarrior()
    {
        if (!isSpawningWarrior && warriorQueue.Count > 0) // Pokud není spuštěné spawnování
        {
            StartCoroutine(SpawnWarriorRoutine());
        }
    }

    private void TrySpawnArcher()
    {
        if (!isSpawningArcher && archerQueue.Count > 0) // Pokud není spuštěné spawnování
        {
            StartCoroutine(SpawnArcherRoutine());
        }
    }

    private IEnumerator SpawnWarriorRoutine()
    {
        isSpawningWarrior = true; // Zamezí souběžnému spawnu

        while (warriorQueue.Count > 0)
        {
            GameObject unitToSpawn = warriorQueue.Dequeue();
            Instantiate(unitToSpawn, warriorSpawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawningWarrior = false;
    }

    private IEnumerator SpawnArcherRoutine()
    {
        isSpawningArcher = true; // Zamezí souběžnému spawnu

        while (archerQueue.Count > 0)
        {
            GameObject unitToSpawn = archerQueue.Dequeue();
            Instantiate(unitToSpawn, archerSpawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawningArcher = false;
    }
}
