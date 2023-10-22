using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyRanged : EnemyBehavior
{
    [SerializeField] private float desiredDistance = 10f;
    [SerializeField] private float attackCD = 1f;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firingSpot;
    [SerializeField] private LayerMask blockableLayers;
    private GameObject bulletInstance;
    private float attackTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        calculateTargetPos();
        if (alert)
        {
            rotateToPlayer();
        }
        else
        {
            rotateToTarget();
        }
        moveToTarget();

        if(hasLineOfSight())
        {
            shootGun();
        }
    }

    private void FixedUpdate()
    {
        attackTimer -= Time.deltaTime;
    }
    private void calculateTargetPos()
    {
        if (alert)
        {
            targetPos = playerPos.position + ((transform.position - playerPos.position).normalized * desiredDistance);
        }
    }

    private void shootGun()
    {
        if (attackTimer <= 0)
        {
            attackTimer = attackCD;
            bulletInstance = Instantiate(bullet, firingSpot.position, transform.rotation);
        }            
    }

    private bool hasLineOfSight()
    {
        RaycastHit2D? hit = Physics2D.Raycast(firingSpot.position, -(transform.position - playerPos.position).normalized, Mathf.Infinity, blockableLayers);
        if(hit != null)
        {
            return ((RaycastHit2D) hit).collider.gameObject.tag == "Player";
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(firingSpot.position, (transform.position - playerPos.position).normalized * -10f) ;
    }
}
