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
        Vector3 newPos = new Vector3(player_x, player_y, 0);
        playerPos.position = newPos;
        mouse_x = Input.mousePosition.x;
        mouse_y = Input.mousePosition.y;
    }

    private void movePlayer()
    {
        float xMovement = playerSpeed * Input.GetAxisRaw("Horizontal");
        float yMovement = playerSpeed * Input.GetAxisRaw("Vertical");
        if ((xMovement > 0 && (colR || colUR || colDR)) || (xMovement < 0 && (colL || colUL || colDL)))
        {
            if(!((colUR && colU && colUL) || (colDR && colDL && colD)))
            {
                xMovement = 0;
            }
        }
        if ((yMovement > 0 && (colU || colUR || colUL)) || (yMovement < 0 && ((colD || colDL || colDR))))
        {
            if (!((colUR && colR && colDR) || (colUL && colDL && colL)))
            {
                yMovement = 0;
            }
        }
        player_x += xMovement;
        player_y += yMovement;
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
