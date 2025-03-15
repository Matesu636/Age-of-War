using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Animator animator;
    BoxCollider2D boxCollider2D;


    public float Speed = 1f;
    private bool canMove = true;

    //private bool isGrounded;
    //private bool isAttacking;

    public bool isPlayerUnit;
    public float hpWarrior = 100;
    public int damage = 20;

    
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
            //animator.SetBool("Grounded", true);
        }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject)
        {
            canMove = true;
            //animator.SetBool("Grounded", true);
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
            //animator.SetBool("Grounded", false);

        }


        Debug.Log("Kolize detekována s: " + boxCollision2D.gameObject.name);
        if (boxCollision2D.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Player"))
        {

            // Pokud jsme útočili na základnu, přestaneme
            CancelInvoke(nameof(AttackBase));

            // Začneme útočit na nepřítele
            InvokeRepeating(nameof(TakeDamage), 0f, 2f);


        }

        if (boxCollision2D.gameObject.CompareTag("EnemyBaseHP"))
        {
            InvokeRepeating(nameof(AttackBase), 0f, 2f); // Útok každé 2 sekundy
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
    public void AttackBase()
    {
        EnemyBase enemyBase = GameObject.FindGameObjectWithTag("EnemyBaseHP").GetComponent<EnemyBase>();
        if (enemyBase != null)
        {
            enemyBase.TakeDamage(damage);
        }
    }




}
