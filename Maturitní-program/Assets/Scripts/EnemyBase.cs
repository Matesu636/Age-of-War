using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyBase : MonoBehaviour
{
    public float enemyBaseHealth = 500; // Životy základny
    public Slider healthSlider;


    public GameObject enemyWarrPrefab;  // Prefab pro běžného nepřítele (Warrior)
    public GameObject enemyArcherPrefab; // Prefab pro Archera

    public GameObject winScreen;

    public Transform spawnPosArcher;
    public Transform spawnPosWarrior;
    private float spawnInterval;

    private float targetTime;

    // Start is called before the first frame update
    void Start()
    {
        
        targetTime = 3;

        if (healthSlider != null)
        {
            healthSlider.maxValue = enemyBaseHealth;
            healthSlider.value = enemyBaseHealth;
        }
    }
    private void Update()
    {
        targetTime -= Time.deltaTime;
        if (targetTime <= 0)
        {
            SpawnEnemy();
            targetTime = Random.Range(5, 15);
            
        }
        //Debug.Log(targetTime);

    }

    

    public void TakeDamage(float damage)
    {
        enemyBaseHealth -= damage;
        Debug.Log("Nepřátelská základna dostala damage: " + damage + ". Zbývající HP: " + enemyBaseHealth);

        if (healthSlider != null)
        {
            healthSlider.value = enemyBaseHealth;
        }

        if (enemyBaseHealth <= 0)
        {
            DestroyBase();
        }
    }

    void DestroyBase()
    {
        Debug.Log("Nepřátelská základna byla zničena!");
        Destroy(gameObject); // Zničí základnu
        winScreen.gameObject.SetActive(true);
        Time.timeScale = 0f;//Zasdtaví čas
    }

    private void SpawnEnemy()
    {
        GameObject enemyToSpawn;
        Transform spawnPos;

        // 1/3 šance na spawn Archera, jinak se spawne Warrior
        if (Random.value < 0.33f)
        {
            enemyToSpawn = enemyArcherPrefab;
            spawnPos = spawnPosArcher;
            Debug.Log("🟢 Spawnován EnemyArcher!");
        }
        else
        {
            spawnPos = spawnPosWarrior;
            enemyToSpawn = enemyWarrPrefab;
            Debug.Log("🔴 Spawnován EnemyWarrior!");
        }

        Instantiate(enemyToSpawn, spawnPos.position, Quaternion.identity);
    }
}
