using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WizzMovement : MonoBehaviour
{
    private Animator animator;
    BoxCollider2D boxCollider2D;


    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private LayerMask enemyMask;
    private Transform target;

    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private Transform firingPoint;

    [SerializeField] private float bps = 1f;
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

        if (!CheckTarget())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }

        }

    }

    private bool CheckTarget()
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
        if (gameObject.CompareTag("Player"))
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
        CancelInvoke(nameof(Shoot));

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
        if (boxCollision2D.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Player"))
        {

            InvokeRepeating(nameof(Shoot), 0f, 2f);


        }

        if (boxCollision2D.gameObject.CompareTag("EnemyBaseHP"))
        {
            InvokeRepeating(nameof(AttackBase), 0f, 2f); // Útok každé 2 sekundy
        }
    }


    private void Shoot()
    {
        GameObject orbObj = Instantiate(orbPrefab, firingPoint.position, Quaternion.identity);
        OrbSpawn orbScript = orbObj.GetComponent<OrbSpawn>();
        orbScript.SetTarget(target);
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
