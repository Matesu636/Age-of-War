using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcherMovement : MonoBehaviour
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
    private bool isInPlayerBase = false;
    private bool isDead = false;

    public bool isPlayerUnit;
    public float hpWarrior = 100;
    public int damage = 20;
    public int gainCoin = 20;

    private void Start()
    {
        animator = GetComponent<Animator>();
        InvokeRepeating(nameof(FindTarget),0f,1);
    }

    private void Update()
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
        if (!CheckTargetIsInRange())
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
        EnemyArrow arrowScript = arrowObj.GetComponent<EnemyArrow>();
        arrowScript.SetTarget(target);
    }

    private bool CheckTargetIsInRange()
    {
        return target != null && Vector2.Distance(transform.position, target.position) <= targetingRange;
    }

    private void FindTarget()
    {
        //  Nejprve najde nejbližšího nepřítele
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Player") || enemy.CompareTag("Archer"))
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
            return; // ✅ Útoč na nepřítele
        }

        // 2. Pokud není nepřítel, najdi EnemyBase
        GameObject baseObj = GameObject.FindGameObjectWithTag("BaseHP");
        if (baseObj != null && Vector2.Distance(transform.position, baseObj.transform.position) <= targetingRange)
        {
            target = baseObj.transform; // Útočí na základnu
        }
        else
        {
            target = null;
        }
    }
    //[2]


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
        if(collision.gameObject)
        {
            if(isInPlayerBase == false)
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
        if (collision.CompareTag("BaseHP"))
        {
            canMove = false;
            animator.SetBool("isRunning", false);
            isInPlayerBase = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("BaseHP"))
        {
            isInPlayerBase = false;
           
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
        if (isDead) return; // Zabrání opakovanému volání
        isDead = true;

        canMove = false;
        Destroy(gameObject, 1f);
        GameManager.Instance.AddGold(isPlayerUnit, gainCoin);
        animator.SetBool("isDead", true);
    }
}
