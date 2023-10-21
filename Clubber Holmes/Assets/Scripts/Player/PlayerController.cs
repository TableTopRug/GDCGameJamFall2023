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
    [SerializeField] private LayerMask impassableLayers;
    [SerializeField] private float playerSpeed = 0.25f;

    [SerializeField] private float collisionDetectionLength = 0.55f;
    [SerializeField] private float collisionDetectionLengthDiagonal = 0.5f;

    private float player_x;
    private float player_y;
    private float mouse_x;
    private float mouse_y;

    public bool colL, colR, colU, colD, colUR, colUL, colDR, colDL;
    private ContactFilter2D collisionFilter;
    private RaycastHit2D[] collisionHits;

    // Start is called before the first frame update
    void Start()
    {
        collisionFilter.SetLayerMask(impassableLayers);
        collisionHits = new RaycastHit2D[10];
    }
    // Update is called once per frame
    void Update()
    {
        updatePositions();
    }
    private void FixedUpdate()
    {
        checkCollisions();
        rotatePlayer();
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

    private void checkCollisions()
    {
        colR = Physics2D.Raycast((Vector2) transform.position, Vector2.right, collisionFilter, collisionHits, collisionDetectionLength) != 0;
        colL = Physics2D.Raycast((Vector2)transform.position, Vector2.left, collisionFilter, collisionHits, collisionDetectionLength) != 0;
        colU = Physics2D.Raycast((Vector2)transform.position, Vector2.up, collisionFilter, collisionHits, collisionDetectionLength) != 0;
        colD = Physics2D.Raycast((Vector2)transform.position, Vector2.down, collisionFilter, collisionHits, collisionDetectionLength) != 0;
        colUR = 0 !=
            Physics2D.Raycast((Vector2)transform.position, new Vector2(1, 1), collisionFilter, collisionHits, collisionDetectionLengthDiagonal)
            + Physics2D.Raycast((Vector2)transform.position, new Vector2(2, 1), collisionFilter, collisionHits, collisionDetectionLengthDiagonal)
            + Physics2D.Raycast((Vector2)transform.position, new Vector2(1, 2), collisionFilter, collisionHits, collisionDetectionLengthDiagonal);
        colUL = 0 !=
            Physics2D.Raycast((Vector2)transform.position, new Vector2(-1, 1), collisionFilter, collisionHits, collisionDetectionLengthDiagonal)
            + Physics2D.Raycast((Vector2)transform.position, new Vector2(-2, 1), collisionFilter, collisionHits, collisionDetectionLengthDiagonal)
            + Physics2D.Raycast((Vector2)transform.position, new Vector2(-1, 2), collisionFilter, collisionHits, collisionDetectionLengthDiagonal);
        colDR = 0 !=
            Physics2D.Raycast((Vector2)transform.position, new Vector2(1, -1), collisionFilter, collisionHits, collisionDetectionLengthDiagonal)
            + Physics2D.Raycast((Vector2)transform.position, new Vector2(2, -1), collisionFilter, collisionHits, collisionDetectionLengthDiagonal)
            + Physics2D.Raycast((Vector2)transform.position, new Vector2(1, -2), collisionFilter, collisionHits, collisionDetectionLengthDiagonal);
        colDL = 0 !=
            Physics2D.Raycast((Vector2)transform.position, new Vector2(-1, -1), collisionFilter, collisionHits, collisionDetectionLengthDiagonal)
            + Physics2D.Raycast((Vector2)transform.position, new Vector2(-2, -1), collisionFilter, collisionHits, collisionDetectionLengthDiagonal)
            + Physics2D.Raycast((Vector2)transform.position, new Vector2(-1, -2), collisionFilter, collisionHits, collisionDetectionLengthDiagonal);
    }

    private void rotatePlayer()
    {
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position).normalized;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Debug.Log(angle);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector2.right * collisionDetectionLength);
        Gizmos.DrawRay(transform.position, Vector2.left * collisionDetectionLength);
        Gizmos.DrawRay(transform.position, Vector2.up * collisionDetectionLength);
        Gizmos.DrawRay(transform.position, Vector2.down * collisionDetectionLength);
        Gizmos.DrawRay(transform.position, new Vector2(Mathf.Sqrt(2)/2, Mathf.Sqrt(2) / 2) * collisionDetectionLengthDiagonal);
        Gizmos.DrawRay(transform.position, new Vector2(-Mathf.Sqrt(2) / 2, Mathf.Sqrt(2) / 2) * collisionDetectionLengthDiagonal);
        Gizmos.DrawRay(transform.position, new Vector2(Mathf.Sqrt(2) / 2, -Mathf.Sqrt(2) / 2) * collisionDetectionLengthDiagonal);
        Gizmos.DrawRay(transform.position, new Vector2(-Mathf.Sqrt(2) / 2, -Mathf.Sqrt(2) / 2) * collisionDetectionLengthDiagonal);
    }
}
