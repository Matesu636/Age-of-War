using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTurret : MonoBehaviour
{
    public GameObject turretGO;
    public Transform spawnPoint;
    public GameObject TurretManager;

    private GameManager gm;


    

    private void Start()
    {
        gm = GameManager.Instance;
    }
    

    public void SpawningTurret()
    {
        if (gm.PlayerGold > 99)
        {
            if (GameManager.Instance.SubtractGold(100))

            {
                Instantiate(turretGO, spawnPoint.position, Quaternion.identity);

            }
        }
    }

    

    public void MenuShow()
    {
        TurretManager.SetActive(true);
    }

    public void MenuHide()
    {
        TurretManager.SetActive(false);
    }

}
