using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTurret : MonoBehaviour
{
    public GameObject turretGO;
    public Transform spawnPoint;

    private GameManager gm;


    

    private void Start()
    {
        gm = GameManager.Instance;
    }
    

    public void SpawningTurret()
    {
        if (gm.PlayerGold > 49)
        {
            if (GameManager.Instance.SubtractGold(50))

            {
                Instantiate(turretGO, spawnPoint.position, Quaternion.identity);



            }
        }
    }

}
