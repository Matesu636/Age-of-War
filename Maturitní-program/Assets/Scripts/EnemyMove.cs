using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private Animator animator;
    BoxCollider2D collision;
    private MonoBehaviour currentEnemy;

    public float Speed = 1f;
    private bool canMove = true;
    private bool isInEnemyBase = false;

    public bool isPlayerUnit;
    public float hpWarrior = 100;
    public float damage = 20;
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
            if (isInEnemyBase == false)
            {
                canMove = true;

                //CancelInvoke(nameof(DealDamage));
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


        Debug.Log("Kolize detekována s: " + boxCollision2D.gameObject.name);
        if ((boxCollision2D.gameObject.CompareTag("Player") || boxCollision2D.gameObject.CompareTag("Archer")) && gameObject.CompareTag("Enemy"))
        {
            CancelInvoke(nameof(AttackBase));

            // Získání správného nepřítele (buď `Movement` nebo `WizzMovement`)
            Movement warrior = boxCollision2D.gameObject.GetComponent<Movement>();
            WizzMovement archer = boxCollision2D.gameObject.GetComponent<WizzMovement>();

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
        if (collision.gameObject.CompareTag("EnemyBaseHP"))
        {
            canMove = true;
            animator.SetBool("isRunning", true);

        }

        if (collision.gameObject.CompareTag("BaseHP"))
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

    private void RepeatDealDamage()
    {
        if (currentEnemy != null)
        {
            if (currentEnemy is Movement warrior)
            {
                warrior.TakeDamage(damage);
            }
            else if (currentEnemy is WizzMovement archer)
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
            return;
        }
    }

    public void Die()
    {
        canMove = false;
        animator.SetBool("isDead", true); // Animace smrti
        Destroy(gameObject, 1f); // Zničí objekt po 1s
        GameManager.Instance.AddGold(isPlayerUnit, gainCoin);

        CancelInvoke(nameof(RepeatDealDamage));
        CancelInvoke(nameof(AttackBase));
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