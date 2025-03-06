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
    public float damage = 20;

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
        if (collision.gameObject.CompareTag("Player"))
        {
            CanMove = true;
        }
        CancelInvoke(nameof(ApplyDamage));
    }

    private void OnCollisionEnter2D(Collision2D boxCollision2D)
    {
        if (boxCollision2D.gameObject.CompareTag("Player"))
        {
            CanMove = false;

        }


        Debug.Log("Kolize detekována s: " + boxCollision2D.gameObject.name);
        if (boxCollision2D.gameObject.CompareTag("Player") && gameObject.CompareTag("Enemy"))
        {

            InvokeRepeating(nameof(ApplyDamage), 0f, 1.9f);


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
    
}
