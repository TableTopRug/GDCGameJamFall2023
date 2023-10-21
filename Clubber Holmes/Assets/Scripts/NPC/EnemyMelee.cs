using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMelee : EnemyBehavior
{
    // Start is called before the first frame update
    void Start()
    {
        
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
    }

    private void calculateTargetPos()
    {
        if(alert)
        {
            targetPos = playerPos.position;
        }
    }
}
