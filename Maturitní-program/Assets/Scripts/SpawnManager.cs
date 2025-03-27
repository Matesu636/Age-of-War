using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int warriorCost = 20;
    public int archerCost = 30;

    public GameObject warriorPrefab;
    public GameObject archerPrefab;

    public Transform warriorSpawnPoint; // Spawn point pro Warriory
    public Transform archerSpawnPoint;  // Spawn point pro Archery

    private Queue<(GameObject unit, Transform spawnPoint)> unitQueue = new Queue<(GameObject, Transform)>(); // Fronta jednotek se spawnpointy
    private bool isSpawning = false;

    public float spawnInterval = 3f; // Časové zpoždění mezi jednotkami

    public void SpawnWarrior()
    {
        if (GameManager.Instance.SubtractGold(warriorCost))
        {
            unitQueue.Enqueue((warriorPrefab, warriorSpawnPoint)); // Přidá Warriora do fronty s jeho spawnpointem
            TrySpawnUnit();
        }
    }

    public void SpawnArcher()
    {
        if (GameManager.Instance.SubtractGold(archerCost))
        {
            unitQueue.Enqueue((archerPrefab, archerSpawnPoint)); // Přidá Archera do fronty s jeho spawnpointem
            TrySpawnUnit();
        }
    }

    private void TrySpawnUnit()
    {
        if (!isSpawning && unitQueue.Count > 0)
        {
            StartCoroutine(SpawnUnitRoutine());
        }
    }

    private IEnumerator SpawnUnitRoutine()
    {
        isSpawning = true;

        while (unitQueue.Count > 0)
        {
            var (unitToSpawn, spawnPoint) = unitQueue.Dequeue(); // Vybere jednotku a její spawn point
            Instantiate(unitToSpawn, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
    }
    //[5]
}
