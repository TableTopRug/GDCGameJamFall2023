using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : EnemyBehavior
{
    [SerializeField] private float desiredDistance = 10f;
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
    }
    private void calculateTargetPos()
    {
        if (alert)
        {
            targetPos = playerPos.position + ((transform.position - playerPos.position).normalized * desiredDistance);
        }
    }
}
