using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Base : MonoBehaviour
{
    public float baseHealth = 500; // Životy základny
    public Slider healthSlider;

    public GameObject loseScreen;

    private void Start()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = baseHealth;
            healthSlider.value = baseHealth;
        }
    }
    public void TakeDamage(float damage)
    {
        baseHealth -= damage;
        Debug.Log("Nepřátelská základna dostala damage: " + damage + ". Zbývající HP: " + baseHealth);

        if (healthSlider != null)
        {
            healthSlider.value = baseHealth;
        }

        if (baseHealth <= 0)
        {
            DestroyBase();
        }
    }

    void DestroyBase()
    {
        Debug.Log("Nepřátelská základna byla zničena!");
        Destroy(gameObject); // Zničí základnu
        loseScreen.gameObject.SetActive(true);
        Time.timeScale = 0f;

    }
}
