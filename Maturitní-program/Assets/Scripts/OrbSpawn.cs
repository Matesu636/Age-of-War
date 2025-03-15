using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSpawn : MonoBehaviour
{
    private Transform target;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float orbSpeed = 5f;
    [SerializeField] private float orbDamage = 20f;


    private void Start()
    {
        
    
    }
    public void SetTarget(Transform _target)
    {
        target = _target;

    }

    private void FixedUpdate()
    {
        

        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * orbSpeed;

        
    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyMove enemy = collision.gameObject.GetComponent<EnemyMove>();
        if (enemy != null)
        {
            enemy.TakeDamage(orbDamage);
        }

        Destroy(gameObject);
    }
}
