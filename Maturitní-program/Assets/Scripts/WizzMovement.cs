using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WizzMovement : MonoBehaviour
{
    private Animator animator;
    BoxCollider2D boxCollider2D;


    [SerializeField] private float targetingRange = 10f;
    [SerializeField] private LayerMask enemyMask;
    private Transform target;

    [SerializeField] private GameObject arrrowPrefab;
    [SerializeField] private Transform firingPoint;

    [SerializeField] private float bps = 2f; //LZE UDELAT PRES INVOKE REPEATING!!!!!
    private float timeUntilFire;


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
        if (target == null)
        {
            FindTarget();
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

            if (timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }

    }

    private void Shoot()
    {
        GameObject arrowObj = Instantiate(arrrowPrefab, firingPoint.position, Quaternion.identity);
        Arrow arrowScript = arrowObj.GetComponent<Arrow>();
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
        if (gameObject.CompareTag("Archer"))
        {
            transform.position += Vector3.right * Speed * Time.deltaTime;

        }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject)
        {
            canMove = true;

        }


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


        }


        Debug.Log("Kolize detekována s: " + boxCollision2D.gameObject.name);
        if (boxCollision2D.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Archer"))
        {


            InvokeRepeating(nameof(ApplyDamage), 0f, 2f);


        }

        if (boxCollision2D.gameObject.CompareTag("EnemyBaseHP"))
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

            Die();
        }

    }

    private void Die()
    {
        canMove = false;
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
