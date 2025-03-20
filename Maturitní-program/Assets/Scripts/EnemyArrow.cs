using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
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
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * arrowSpeed;

        if (target = null)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Movement enemy = collision.gameObject.GetComponent<Movement>();
        if (enemy != null)
        {
            
            enemy.TakeDamage(arrowDamage);
        }
        WizzMovement archer = collision.gameObject.GetComponent<WizzMovement>();
        if (archer != null)
        {

            archer.TakeDamage(arrowDamage);
        }
        Base _base = collision.gameObject.GetComponent<Base>();
        if (_base != null)
        {
            _base.TakeDamage(arrowDamage);
        }

        Destroy(gameObject);
    }
}
