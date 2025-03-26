using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform rotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bulletDamage = 20f;  

    private Transform target;


    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [SerializeField] private float bps = 1f;
    private float timeUntilFire;


    private void Start()
    {
        GameManager.Instance.SetTurret(this);
    }

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }
        RotateToTarget();

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

    private void RotateToTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        rotationPoint.rotation = Quaternion.RotateTowards(rotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void IncreaseDamage(float amount)
    {
        bulletDamage += amount;
        
    }

    public float GetDamage()
    {
        return bulletDamage;
    }

    private void Shoot()
    {
        Debug.Log("Turret střílí!");
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        

        if (bulletScript != null)
        {
            bulletScript.SetDamage(bulletDamage);
            bulletScript.SetTarget(target);
            Debug.Log("Střela má správný cíl: " + target.name);
        }
    }

    private void OnMouseDown()
    {
        Destroy(gameObject);
    }
}
