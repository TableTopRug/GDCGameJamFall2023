using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMelee : EnemyBehavior
{
    private ContactFilter2D playerFilter;
    // Start is called before the first frame update
    void Start()
    {
        playerFilter = new ContactFilter2D();
        playerFilter.SetLayerMask(playerLayer);
    }

    // Update is called once per frame
    void Update()
    {
        calculateTargetPos();
        if(alert)
        {
            rotateToPlayer();
        } else
        {
            rotateToTarget();
        }
        moveToTarget();

        if(hitbox.IsTouchingLayers(playerLayer))
        {
            stun(0.25f);
            Collider2D[] playerCollider = new Collider2D[1];
            hitbox.GetContacts(playerFilter, playerCollider);
            GameObject player = playerCollider[0].gameObject;
            player.GetComponent<CombatHandler>().takeDamage();
            player.GetComponent<PlayerController>().knockbackPlayer(playerCollider[0].ClosestPoint(transform.position));
        }
    }

    private void calculateTargetPos()
    {
        if(alert)
        {
            targetPos = playerPos.position;
        }
    }
}
