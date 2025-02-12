using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWarrior : MonoBehaviour
{
    public GameObject Warrior;
    public Transform SpawnPoint;


    public void SpawningWarrior()
    {
        if(GameManager.Instance.PlayerGold > 20)
        {
            Instantiate(Warrior, SpawnPoint.position, Quaternion.identity);
            GameManager.Instance.PlayerGold -= 20;
        }
            
        
    }

    
}

