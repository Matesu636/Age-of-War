using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public int baseHealth = 500; // Životy základny

    public void TakeDamage(int damage)
    {
        baseHealth -= damage;
        Debug.Log("Nepřátelská základna dostala damage: " + damage + ". Zbývající HP: " + baseHealth);

        if (baseHealth <= 0)
        {
            DestroyBase();
        }
    }

    void DestroyBase()
    {
        Debug.Log("Nepřátelská základna byla zničena!");
        Destroy(gameObject); // Zničí základnu
    }
}
