using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    
    public Transform playerPos;

    [SerializeField] private Collider2D playerHitbox;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float stunTime;
    [SerializeField] private float knockbackForce;
    private float stunTimer;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (stunTimer <= 0)
        {
            rotatePlayer();
            movePlayer();
        }
        stunTimer -= Time.deltaTime;
    }

    public void knockbackPlayer(Vector3 knockbackPoint)
    {
        //do knockback stuff
        stunTimer = stunTime;
    }

    private void movePlayer()
    {
        Vector2 inputMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.velocity = inputMovement.normalized * playerSpeed;
    }

    public void setPlayerSpeed(float newSpeed)
    {
        playerSpeed = newSpeed;
    }

    private void rotatePlayer()
    {
        Vector2 direction = -(transform.position-Camera.main.ScreenToWorldPoint(Input.mousePosition)).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

}
