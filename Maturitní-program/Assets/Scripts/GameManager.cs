using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI textGold;
    public TMP_Text textTime;

    public bool timeIsRunning;
    public float timeRemaining;

    private Turret turret;
    private WizzMovement wizz;

    public int PlayerGold = 100;
    public int time;


    private void Start()
    {
        timeIsRunning = true;
        textGold.text = PlayerGold.ToString();
    }

    private void Update()
    {
        if(timeIsRunning)
        {
            if (timeRemaining >= 0)
            {
                timeRemaining += Time.deltaTime;
                DisplayTime(timeRemaining);
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        textTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
    }

    

    public bool SubtractGold(int gold)
    {
        if (gold > 0)
        {
            if (PlayerGold >= gold)
            {
                PlayerGold -= gold;
                textGold.text = PlayerGold.ToString();
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
                textGold.text = PlayerGold.ToString();
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
            textGold.text = PlayerGold.ToString();
            turret.IncreaseDamage(10); // Zvýší damage o 10
            Debug.Log("🔼 Turret damage upgradován! Nové damage: " + turret.GetDamage());
        }
        else
        {
            Debug.Log("❌ Nedostatek zlata nebo turret neexistuje!");
        }




    }
}
