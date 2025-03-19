using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI text;



    private Turret turret;

    private WizzMovement wizz;

    public int PlayerGold = 100;


    private void Start()
    {
        text.text = PlayerGold.ToString();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
    }
    /// <summary>
    /// Checks if the player has enough money and subtracts it if yes
    /// </summary>
    /// <param name="gold"></param>
    /// <returns>True if subtraction was successfull</returns>
    public bool SubtractGold(int gold)
    {
        if (gold > 0)
        {
            if (PlayerGold >= gold)
            {
                PlayerGold -= gold;
                text.text = PlayerGold.ToString();
                return true;

            }

            return false;
        }
        else
        {

            return false;
        }

    }

    public void AddGold(bool isPlayerUnit, int gold)
    {
        if (!isPlayerUnit)
        {
            if (gold > 0)
            {
                PlayerGold += gold;
                text.text = PlayerGold.ToString();
            }
            else
            {
                return;
            }

        }
            

    }

    public void SetTurret(Turret t)
    {
        turret = t;
    }

    public void SetWizz(WizzMovement w)
    {
        wizz = w;
    }



    public void UpgradeTurretDamage()
    {
        int upgradeCost = 50; // Cena upgradu

        if (PlayerGold >= upgradeCost && turret != null)
        {
            PlayerGold -= upgradeCost;
            text.text = PlayerGold.ToString();
            turret.IncreaseDamage(10); // Zvýší damage o 10
            Debug.Log("🔼 Turret damage upgradován! Nové damage: " + turret.GetDamage());
        }
        else
        {
            Debug.Log("❌ Nedostatek zlata nebo turret neexistuje!");
        }




    }
}
