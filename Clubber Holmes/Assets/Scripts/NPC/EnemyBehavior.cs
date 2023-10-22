using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    
    public bool alert;
    public Vector3 targetPos;
    public float speed;
    public Rigidbody2D rb;
    public Collider2D hitbox;
    public bool canMove = true;
    public int health = 3;
    public LayerMask playerLayer;
    private float lazyVal = 0.5f;
    private float stunTimer = 0f;
    public Transform playerPos;


    public void rotateToTarget()
    {
        Vector2 direction = (targetPos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    public void rotateToPlayer()
    {
        Vector2 direction = (playerPos.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    public void moveToTarget()
    {
        if(canMove)
        {
            Vector3 diff = targetPos - transform.position;
            if (diff.magnitude > lazyVal)
            {
                rb.velocity = diff.normalized * speed;
            }
        }
    }

    private void FixedUpdate()
    {
        canMove = stunTimer <= 0;
        stunTimer -= Time.deltaTime;
    }

    public void getHit(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void stun(float stunTime)
    {
        stunTimer = stunTime;
    }
}
