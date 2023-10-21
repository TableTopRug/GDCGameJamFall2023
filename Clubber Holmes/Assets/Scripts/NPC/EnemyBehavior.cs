using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform playerPos;
    public bool alert;
    public Vector3 targetPos;
    public float speed;
    public Rigidbody2D rb;

    private float lazyVal = 0.5f;

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
        Vector3 diff = targetPos - transform.position;
        if(diff.magnitude > lazyVal)
        {
            rb.velocity = diff.normalized * speed;
        }
    }
}
