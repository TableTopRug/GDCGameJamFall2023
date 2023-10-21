using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    
    public Transform playerPos;

    [SerializeField] private Collider2D playerHitbox;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private float playerSpeed = 0.25f;

    private float player_x;
    private float player_y;
    private float mouse_x;
    private float mouse_y;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
   
        updatePositions();
    }
    private void FixedUpdate()
    {
        movePlayer();
    }
    private void updatePositions()
    {
        Vector3 newPos = new Vector3(player_x, player_y, 0);
        playerPos.position = newPos;
        mouse_x = Input.mousePosition.x;
        mouse_y = Input.mousePosition.y;
    }

    private void movePlayer()
    {
        player_x += playerSpeed * Input.GetAxisRaw("Horizontal");
        player_y += playerSpeed * Input.GetAxisRaw("Vertical");
    }

    public void setPlayerSpeed(float newSpeed)
    {
        playerSpeed = newSpeed;
    }
}
