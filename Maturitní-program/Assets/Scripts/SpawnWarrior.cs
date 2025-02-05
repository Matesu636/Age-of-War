using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWarrior : MonoBehaviour
{
    public GameObject Warrior;
    public Transform Spawn;


    public void SpawningWarrior()
    {
        if(true)
        {
            Instantiate(Warrior, Spawn.position, Quaternion.identity);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

