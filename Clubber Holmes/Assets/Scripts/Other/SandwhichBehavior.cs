using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandwhichBehavior : MonoBehaviour
{
    [SerializeField] private LayerMask consumeOnContact;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((consumeOnContact.value & (1 << collision.gameObject.layer)) > 0)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<CombatHandler>().heal(1);
            }
            Destroy(gameObject);
        }
    }
}
