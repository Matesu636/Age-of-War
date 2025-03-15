using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    public int enemyBaseHealth = 500; // Životy základny
    public Slider healthSlider;


    public GameObject enemyPrefab;
    public Transform spawnPos;
    private float spawnInterval;

    private float targetTime;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
        targetTime = 15;

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
            //Debug.Log(targetTime);
        }
        //Debug.Log(targetTime);

    }

    

    public void TakeDamage(int damage)
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
    }

    private void SpawnEnemy()
    {
        spawnInterval = Random.Range(1f, 3.5f);
        Instantiate(enemyPrefab, spawnPos.position, Quaternion.identity);
    }
}
