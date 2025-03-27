using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] public float bulletDamage = 20f;

    

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyMove enemy = collision.gameObject.GetComponent<EnemyMove>();
        if (enemy != null)
        {
            enemy.TakeDamage(bulletDamage);
        }

        Destroy(gameObject);
    }

    public void SetDamage(float damage)
    {
        bulletDamage = damage;
    }


}
//[1]
