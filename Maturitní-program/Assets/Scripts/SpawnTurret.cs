using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTurret : MonoBehaviour
{
    public GameObject Turret;
    public Transform SpawnPoint;

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
                Instantiate(Turret, SpawnPoint.position, Quaternion.identity);



            }
        }


    }

}
