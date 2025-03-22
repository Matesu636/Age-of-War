using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizzMovement : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private float targetingRange = 10f;
    [SerializeField] private LayerMask enemyMask;
    private Transform target;

    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firingPoint;

    [SerializeField] private float bps = 2f;
    private float timeUntilFire;

    public float Speed = 1f;
    private bool canMove = true;
    private bool isInEnemyBase = false;

    public bool isPlayerUnit;
    public float hpWarrior = 100;
    public int damage = 20;

    private void Start()
    {
        animator = GetComponent<Animator>();
        InvokeRepeating(nameof(FindTarget), 0f, 1);
    }

    void Update()
    {
        if (canMove)
        {
            Move();
        }

        if (target == null)
        {
            animator.SetBool("isAttacking", false);
            return;
        }
        if (!CheckTargetInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;
            if (hpWarrior > 0 && timeUntilFire >= 1f / bps)
            {
                Shoot();
                animator.SetBool("isAttacking", true);
                timeUntilFire = 0f;
            }
        }
    }

    private void Shoot()
    {
        if (target == null) return;

        GameObject arrowObj = Instantiate(arrowPrefab, firingPoint.position, Quaternion.identity);
        Arrow arrowScript = arrowObj.GetComponent<Arrow>();
        arrowScript.SetTarget(target);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
        
    }

    private bool CheckTargetInRange()
    {
        return target != null && Vector2.Distance(transform.position, target.position) <= targetingRange;
    }

    private void Move()
    {
        if (gameObject.CompareTag("Archer"))
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
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject)
        {
            canMove = false;
            animator.SetBool("isRunning", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBaseHP"))
        {
            canMove = false;
            animator.SetBool("isRunning", false);
            isInEnemyBase = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBaseHP"))
        {
            isInEnemyBase = false;
            
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
        animator.SetBool("isDead", true);
        Destroy(gameObject, 1f);
    }
}
