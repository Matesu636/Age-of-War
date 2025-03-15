using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    BoxCollider2D boxCollider2D;
    public float Speed = 1f;
    private bool CanMove = true;

    public bool isPlayerUnit;
    public float hpWarrior = 100;
    public int damage = 20;

    

   

    void Update()
    {
        if (CanMove)
        {
            Move();

        }
    }
    private void Move()
    {
        if (gameObject.CompareTag("Enemy"))
            {
                transform.position += Vector3.left* Speed * Time.deltaTime;

            }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject)
        {
            CanMove = true;
        }
        CancelInvoke(nameof(ApplyDamage));

        if (collision.gameObject.CompareTag("BaseHP"))
        {
            CancelInvoke(nameof(AttackBase)); // Přestane útočit, pokud se vzdálí
        }
    }

    private void OnCollisionEnter2D(Collision2D boxCollision2D)
    {
        if (boxCollision2D.gameObject)
        {
            CanMove = false;

        }


        Debug.Log("Kolize detekována s: " + boxCollision2D.gameObject.name);
        if (boxCollision2D.gameObject.CompareTag("Player") && gameObject.CompareTag("Enemy"))
        {

            // Pokud jsme útočili na základnu, přestaneme
            CancelInvoke(nameof(AttackBase));

            // Začneme útočit na nepřítele
            InvokeRepeating(nameof(ApplyDamage), 0f, 1.9f);


        }
        if (boxCollision2D.gameObject.CompareTag("BaseHP"))
        {
            InvokeRepeating(nameof(AttackBase), 0f, 2f); // Útok každé 2 sekundy
        }
    }

    private void ApplyDamage()
    {
        TakeDamage(damage);
    }

    public void TakeDamage(float dmg)
    {
        
        hpWarrior -= dmg;
        if (hpWarrior <= 0)
        {
            GameManager.Instance.AddGold(!isPlayerUnit, 30);
            Destroy(gameObject);
        }

    }

    public void AttackBase()
    {
        Base playerBase = GameObject.FindGameObjectWithTag("BaseHP").GetComponent<Base>();
        if (playerBase != null)
        {
            playerBase.TakeDamage(damage);
        }
    }

    
}
