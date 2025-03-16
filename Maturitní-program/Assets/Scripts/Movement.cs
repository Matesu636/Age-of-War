using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;


    public float Speed = 1f;
    private bool canMove = true;

    

    public bool isPlayerUnit;
    public float hpWarrior = 100;
    public int damage = 20;

    
    private void Start()
    {

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }
    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {

            Move();

        }

    }
    

    private void Move()
    {
        if (gameObject.CompareTag("Player"))
        {
            transform.position += Vector3.right * Speed * Time.deltaTime;
            animator.SetBool("isRunning", true);
        }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject)
        {
            canMove = true;
            
        }
        CancelInvoke(nameof(TakeDamage));

        if (collision.gameObject.CompareTag("EnemyBaseHP"))
        {
            CancelInvoke(nameof(AttackBase)); // Přestane útočit, pokud se vzdálí
        }
    }


    private void OnCollisionEnter2D(Collision2D boxCollision2D)
    {
        if (boxCollision2D.gameObject)
        {
            canMove = false;
            animator.SetBool("isRunning", false);

        }

        if (boxCollision2D.gameObject.CompareTag("Enemy"))
        {
            InvokeRepeating(nameof(AttackEnemy), 0f, 2f);
        }

        //if (boxCollision2D.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Player"))
        //{
        //
        //    // Pokud jsme útočili na základnu, přestaneme
        //    CancelInvoke(nameof(AttackBase));
        //
        //    // Začneme útočit na nepřítele
        //    InvokeRepeating(nameof(TakeDamage), 0f, 2f);
        //
        //    InvokeRepeating(nameof(AttackEnemy),0f,2f);
        //
        //
        //}

        if (boxCollision2D.gameObject.CompareTag("EnemyBaseHP"))
        {
            InvokeRepeating(nameof(AttackBase), 0f, 2f); // Útok každé 2 sekundy
        }
    }

    private void AttackEnemy()
    {
        animator.SetBool("isAttacking", true); // Spustí animaci útoku
    }


    public void TakeDamage(float dmg)
    {

        hpWarrior -= damage;
        animator.SetTrigger("isHurt");
        if (hpWarrior <= 0)
        {
            Die();
        }

    }

    private void Die()
    {
        canMove = false;
        animator.SetBool("isDead", true); // Animace smrti
        Destroy(gameObject, 2f); // Zničí objekt po 2s
    }

    public void AttackBase()
    {
        EnemyBase enemyBase = GameObject.FindGameObjectWithTag("EnemyBaseHP").GetComponent<EnemyBase>();
        if (enemyBase != null)
        {
            enemyBase.TakeDamage(damage);
        }
    }




}
