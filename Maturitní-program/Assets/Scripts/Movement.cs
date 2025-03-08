using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement: MonoBehaviour
{
    BoxCollider2D boxCollider2D;
    public float Speed = 1f;
    private bool CanMove = true;

    public bool isPlayerUnit;
    public float hpWarrior = 100;
    public int damage = 20;

    

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            Move(); 

        }
        
    }

    private void Move()
    {
        if(gameObject.CompareTag("Player"))
        {
            transform.position += Vector3.right * Speed * Time.deltaTime;

        }
        

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject)
            {
                CanMove = true;
            }
        CancelInvoke(nameof(TakeDamage));
    }


    private void OnCollisionEnter2D(Collision2D boxCollision2D)
    {
        if (boxCollision2D.gameObject.CompareTag("Enemy"))
        {
            CanMove = false;
            
        }


        Debug.Log("Kolize detekována s: " + boxCollision2D.gameObject.name);
        if (boxCollision2D.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Player"))
        {

            InvokeRepeating(nameof(TakeDamage), 0f, 2f);


        }
    }


    public void TakeDamage()
    {

        hpWarrior -= damage;
        if (hpWarrior <= 0)
        {
            GameManager.Instance.AddGold(!isPlayerUnit, 30);
            Destroy(gameObject);
        }
        
    }

    
}
