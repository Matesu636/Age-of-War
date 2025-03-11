using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI text;



    //[SerializeField]

    public int PlayerGold = 100;

    public int baseHealth = 300;
    public int enemyBaseHealth = 300;

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
        if (isPlayerUnit)
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




}
