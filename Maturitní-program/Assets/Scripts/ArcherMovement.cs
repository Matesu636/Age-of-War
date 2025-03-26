using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherMovement : MonoBehaviour
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
        int destroyed = PlayerPrefs.GetInt("BasesDestroyed", 0);
        damage = damage + destroyed * 5;

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
        // Nejprve najde nejbližšího nepřítele
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy") || enemy.CompareTag("EnemyArcher"))
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }
        }

        if (closestEnemy != null)
        {
            target = closestEnemy;
            return; // Útočí na nepřítele
        }

        //  Pokud není nepřítel, najde EnemyBase
        GameObject baseObj = GameObject.FindGameObjectWithTag("EnemyBaseHP");
        if (baseObj != null && Vector2.Distance(transform.position, baseObj.transform.position) <= targetingRange)
        {
            target = baseObj.transform; // Útočí na základnu
        }
        else
        {
            target = null;
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
