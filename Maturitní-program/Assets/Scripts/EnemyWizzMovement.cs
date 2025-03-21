using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyWizzMovement : MonoBehaviour
{
    private Animator animator;



    [SerializeField] private float targetingRange = 10f;
    [SerializeField] private LayerMask enemyMask;
    private Transform target;

    [SerializeField] private GameObject arrrowPrefab;
    [SerializeField] private Transform firingPoint;

    [SerializeField] private float bps = 2f; //LZE UDELAT PRES INVOKE REPEATING!!!!!
    private float timeUntilFire;


    public float Speed = 1f;
    private bool canMove = true;
    private bool isInEnemyBase = false;

    //private bool isGrounded;
    //private bool isAttacking;

    public bool isPlayerUnit;
    public float hpWarrior = 100;
    public int damage = 20;
    public int gainCoin = 20;


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
        if (target == null)
        {
            FindTarget();
            CancelInvoke(nameof(AttackEnemy));
            return;
        }

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            //LZE UDELAT PRES INVOKE REPEATING!!!!!

            timeUntilFire += Time.deltaTime;

            if(hpWarrior>=0)
            {
                if (timeUntilFire >= 1f / bps)
                {
                    Shoot();
                    InvokeRepeating(nameof(AttackEnemy), 0f, 3f);
                    timeUntilFire = 0f;
                }

            }
        }

    }

    private void AttackEnemy()
    {
        animator.SetBool("isAttacking", true); // Spustí animaci útoku

    }

    private void Shoot()
    {
        GameObject arrowObj = Instantiate(arrrowPrefab, firingPoint.position, Quaternion.identity);
        EnemyArrow arrowScript = arrowObj.GetComponent<EnemyArrow>();
        arrowScript.SetTarget(target);
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private void Move()
    {
        if (gameObject.CompareTag("EnemyArcher"))
        {
            transform.position += Vector3.left * Speed * Time.deltaTime;
            animator.SetBool("isRunning", true);
        }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject)//Kvuli tomu jde skrz zakladny
        {
            if (isInEnemyBase == false)
            {
                canMove = true;
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
            isInEnemyBase = true;

            InvokeRepeating(nameof(AttackBase), 0f, 2f); // Útok každé 2 sekundy

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
        Destroy(gameObject, 1f); // Zničí objekt po 2s
        GameManager.Instance.AddGold(isPlayerUnit, gainCoin);
        animator.SetBool("isDead", true); // Animace smrti
    }



    public void AttackBase()
    {
        Base _base = GameObject.FindGameObjectWithTag("BaseHP").GetComponent<Base>();
        if (_base != null)
        {
            _base.TakeDamage(damage);
        }
    }




}
