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

        

        if (boxCollision2D.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Player"))
        {
        
            // Začneme útočit na nepřítele
            InvokeRepeating(nameof(ApplyDamage), 0f, 2f);
        
            InvokeRepeating(nameof(AttackEnemy),0f,2f);
        
        
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BaseHP"))
        {
            canMove = true;
            animator.SetBool("isRunning", true);

        }

        if (collision.gameObject.CompareTag("EnemyBaseHP"))
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
        }

    }

    private void Die()
    {
        canMove = false;
        animator.SetBool("isDead", true); // Animace smrti
        Destroy(gameObject, 1f); // Zničí objekt po 1s

        CancelInvoke(nameof(AttackBase));
        CancelInvoke(nameof(ApplyDamage));
    }

    public void AttackBase()
    {
        Base enemyBase = GameObject.FindGameObjectWithTag("EnemyBaseHP").GetComponent<Base>();
        if (enemyBase != null)
        {
            enemyBase.TakeDamage(damage);
        }
    }




}
