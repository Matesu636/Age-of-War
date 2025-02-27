using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWarrior : MonoBehaviour
{
    public GameObject Warrior;
    public Transform SpawnPointWa;

    
    

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance;
    }



    public void SpawningWarrior()
    {
        if (gm.PlayerGold > 19)
        {
            if (GameManager.Instance.SubtractGold(20))

            {
            Instantiate(Warrior, SpawnPointWa.position, Quaternion.identity);



             }
        }


    }

    

    


}

