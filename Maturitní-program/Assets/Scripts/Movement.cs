using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Animator animator;
    private MonoBehaviour currentEnemy;

    public bool isPlayerUnit;

    public float Speed = 1f;
    private bool canMove = true;
    private bool isInEnemyBase = false;



    public float hpWarrior = 100;
    public float damage = 20;


    private void Start()
    {

        animator = GetComponent<Animator>();
        

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
            if (isInEnemyBase == false)
            {
                canMove = true;


                CancelInvoke(nameof(AttackEnemy));

                animator.SetBool("isAttacking", false);
            }
            if (isInEnemyBase == true)
            {
                canMove = false;

                InvokeRepeating(nameof(RepeatDealDamage), 0f, 1.9f); // Opakovaný útok
                InvokeRepeating(nameof(AttackBase), 0f, 1.9f);

                animator.SetBool("isAttacking", true);
            }

        }

    }




    private void OnCollisionEnter2D(Collision2D boxCollision2D)
    {
        if (boxCollision2D.gameObject)
        {
            canMove = false;
            animator.SetBool("isRunning", false);

        }



        if ((boxCollision2D.gameObject.CompareTag("Enemy") || boxCollision2D.gameObject.CompareTag("EnemyArcher")) && gameObject.CompareTag("Player"))
        {

            CancelInvoke(nameof(AttackBase));

            // Získání správného nepřítele 
            EnemyMove warrior = boxCollision2D.gameObject.GetComponent<EnemyMove>();
            EnemyArcherMovement archer = boxCollision2D.gameObject.GetComponent<EnemyArcherMovement>();

            if (warrior != null)
            {
                currentEnemy = warrior; // Uložíme si Warriora
                InvokeRepeating(nameof(RepeatDealDamage), 0f, 1.9f); // Opakovaný útok
                InvokeRepeating(nameof(AttackEnemy), 0f, 1.9f);
            }
            else if (archer != null)
            {
                currentEnemy = archer; // Uložíme si Archera
                InvokeRepeating(nameof(RepeatDealDamage), 0f, 1.9f);
                InvokeRepeating(nameof(AttackEnemy), 0f, 1.9f);
            }
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
            isInEnemyBase = true;

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

    private void RepeatDealDamage()
    {
        if (currentEnemy != null)
        {
            if (currentEnemy is EnemyMove warrior)
            {
                warrior.TakeDamage(damage);
            }
            else if (currentEnemy is EnemyArcherMovement archer)
            {
                archer.TakeDamage(damage);
            }
        }
        else
        {
            CancelInvoke(nameof(RepeatDealDamage)); // Pokud nepřítel zmizí, zastav útoky
        }
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
        CancelInvoke(nameof(RepeatDealDamage));
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
