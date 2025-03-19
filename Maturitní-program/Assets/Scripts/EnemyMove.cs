using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private Animator animator;
    BoxCollider2D boxCollider2D;

    public float Speed = 1f;
    private bool canMove = true;

    public bool isPlayerUnit;
    public float hpWarrior = 100;
    public int damage = 20;
    public int gainCoin = 20;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (canMove)
        {
            Move();

        }
    }
    private void Move()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            transform.position += Vector3.left * Speed * Time.deltaTime;
            animator.SetBool("isRunning", true);
        }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject)
        {
            canMove = true;
        }
        CancelInvoke(nameof(ApplyDamage));
        CancelInvoke(nameof(AttackEnemy));

        animator.SetBool("isAttacking", false);

    }

    

    private void OnCollisionEnter2D(Collision2D boxCollision2D)
    {
        if (boxCollision2D.gameObject)
        {
            canMove = false;
            animator.SetBool("isRunning", false);

        }


        Debug.Log("Kolize detekována s: " + boxCollision2D.gameObject.name);
        if (boxCollision2D.gameObject.CompareTag("Player")|| boxCollision2D.gameObject.CompareTag("Archer") && gameObject.CompareTag("Enemy"))
        {

            // Pokud jsme útočili na základnu, přestaneme
            CancelInvoke(nameof(AttackBase));

            // Začneme útočit na nepřítele
            InvokeRepeating(nameof(ApplyDamage), 0f, 1.9f);
            InvokeRepeating(nameof(AttackEnemy), 0f, 1.9f);


        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemybaseHP"))
        {
            canMove = true;
            animator.SetBool("isRunning", true);

        }

        if (collision.gameObject.CompareTag("BaseHP"))
        {
            canMove = false;
            animator.SetBool("isRunning", false);

            InvokeRepeating(nameof(AttackBase), 0f, 2f); // Útok každé 2 sekundy
            InvokeRepeating(nameof(AttackEnemy), 0f, 2f);
        }
        
    }

    private void AttackEnemy()
    {
        animator.SetBool("isAttacking", true); // Spustí animaci útoku
    }

    private void StopAttacking()
    {
        animator.SetBool("isAttacking", false);
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
            Die();
            return;
        }
    }

    public void Die()
    {
        canMove = false;
        animator.SetBool("isDead", true); // Animace smrti
        Destroy(gameObject, 1f); // Zničí objekt po 1s
        GameManager.Instance.AddGold(isPlayerUnit,gainCoin);
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