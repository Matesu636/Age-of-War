using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Transform target;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float arrowSpeed = 5f;
    [SerializeField] private float arrowDamage = 15;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * arrowSpeed;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != target) return; // 💥 pouze cíl, na který letím

        EnemyMove enemy = collision.gameObject.GetComponent<EnemyMove>();
        if (enemy != null)
        {

            enemy.TakeDamage(arrowDamage);
        }
        EnemyWizzMovement archer = collision.gameObject.GetComponent<EnemyWizzMovement>();
        if (archer != null)
        {

            archer.TakeDamage(arrowDamage);
        }
        EnemyBase _base = collision.gameObject.GetComponent<EnemyBase>();
        if (_base != null)
        {
            _base.TakeDamage(arrowDamage);
        }

        Destroy(gameObject);
    }
}
