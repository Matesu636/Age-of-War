using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArcher : MonoBehaviour
{
    public GameObject Archer;
    public Transform SpawnPointAr;
    private GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance;
    }


    public void SpawningArcher()
    {
        if(gm.PlayerGold > 29)
        {
            if (GameManager.Instance.SubtractGold(30))
            {
                Instantiate(Archer, SpawnPointAr.position, Quaternion.identity);

            }

        }



    }


}

