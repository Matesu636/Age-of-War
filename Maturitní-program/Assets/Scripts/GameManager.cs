using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int PlayerGold = 100;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
    }

    public void AddGold(int Gold)
    {
        PlayerGold += Gold;
    }
}
