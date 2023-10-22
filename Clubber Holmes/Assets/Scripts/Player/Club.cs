using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Club : MonoBehaviour
{
    [SerializeField] private Transform playerPos;
    [SerializeField] private float clubHitboxRadius;
    [SerializeField] private LayerMask hittableLayers;
    [SerializeField] private float clubCD;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float stunTime;
    private float swingTimer;

    private void Start()
    {
        swingTimer = clubCD;
    }

    private void FixedUpdate()
    {
        swingTimer -= Time.deltaTime;
    }

    public void swingClub()
    {
        //if can swing
        if(swingTimer <= 0)
        {
            clubEffects(checkHitbox());
            swingTimer = clubCD;
        }
    }
    
    public void clubEffects(GameObject[] objectsHit)
    {
        //do special stuff to objects
        for (int i = 0; i < objectsHit.Length; i++)
        {
            objectsHit[i].gameObject.GetComponent<EnemyBehavior>().stun(stunTime);
            objectsHit[i].gameObject.GetComponent<EnemyBehavior>().getHit(1);
            Vector2 force = (objectsHit[i].transform.position - playerPos.position).normalized * knockbackForce; 
            Rigidbody2D rb = objectsHit[i].GetComponent<Rigidbody2D>();
            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }

    private GameObject[] checkHitbox()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)transform.position, clubHitboxRadius, hittableLayers);
        GameObject[] hitObjects = new GameObject[hits.Length];
        for(int i = 0; i < hits.Length; i++)
        {
            hitObjects[i] = hits[i].gameObject;
            Debug.Log(hitObjects[i].name);
        }
        return hitObjects;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, clubHitboxRadius);
    }
}
