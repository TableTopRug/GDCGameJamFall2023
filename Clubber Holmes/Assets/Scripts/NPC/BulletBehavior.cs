using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask destroyOnHit;

    private Rigidbody2D rb;
    private Vector3 origin;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((destroyOnHit.value & (1 << collision.gameObject.layer)) > 0)
        {
            if(collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<CombatHandler>().takeDamage();
            }

            Destroy(gameObject);
        }
    }
}
