using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class WitnessAI : MonoBehaviour
{
    [SerializeField] private Tilemap maze;
    [SerializeField] private Grid grid;
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private float collisionDetectionLength;
    [SerializeField] private float speed;
    [SerializeField] private float lockoutTime;
    [SerializeField] private LayerMask playerLayer;
    private float lockoutTimer;
    public bool colL, colR, colU, colD;
    private Vector2 movement;
    private Vector2 currentDirection;
    private ContactFilter2D contactFilter;
    public int curX, curY;
    public Text elemets;
    public float fadeIn = 5.0f;
    public bool isDone = false;


    // Start is called before the first frame update
    void Start()
    {
        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(terrainLayer);
        currentDirection = Vector2.up;
        curX = Mathf.RoundToInt(transform.position.x);
        curY = Mathf.RoundToInt(transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        fadeIn -= Time.deltaTime;

        if (fadeIn <= 0.0f && isDone)
        {
            timerEnded();
        }
    }

    private void FixedUpdate()
    {
        //[L, R, U, D]
        checkCollisions();
        List<Vector2> availableDirections = new List<Vector2>();
        availableDirections.Add(currentDirection);
        if (lockoutTimer < 0)
        {
            if (currentDirection.Equals(Vector2.left))
            {
                if (isCorner(curX, curY))
                {
                    availableDirections.Add(Vector2.up);
                }
                if (isCorner(curX, curY - 1))
                {
                    availableDirections.Add(Vector2.down);
                }
            }
            if (currentDirection.Equals(Vector2.right))
            {
                if (isCorner(curX - 1, curY))
                {
                    availableDirections.Add(Vector2.up);
                }
                if (isCorner(curX - 1, curY - 1))
                {
                    availableDirections.Add(Vector2.down);
                }
            }
            if (currentDirection.Equals(Vector2.up))
            {
                if (isCorner(curX, curY - 1))
                {
                    availableDirections.Add(Vector2.right);
                }
                if (isCorner(curX - 1, curY - 1))
                {
                    availableDirections.Add(Vector2.left);
                }
            }
            if (currentDirection.Equals(Vector2.down))
            {
                if (isCorner(curX, curY))
                {
                    availableDirections.Add(Vector2.right);
                }
                if (isCorner(curX - 1, curY))
                {
                    availableDirections.Add(Vector2.left);
                }
            }
            if (colL && currentDirection.Equals(Vector2.left))
            {
                availableDirections.Clear();
                if (isDeadendPiece(curX - 1, curY - 1) && isDeadendPiece(curX - 1, curY))
                {
                    availableDirections.Add(currentDirection * -1);
                } else
                {
                    availableDirections.Add(Vector2.right);
                    availableDirections.Add(Vector2.down);
                    availableDirections.Add(Vector2.up);
                    availableDirections.Remove(currentDirection * -1);
                }
            }
            if (colR && currentDirection.Equals(Vector2.right))
            {
                availableDirections.Clear();
                if (colR && isDeadendPiece(curX, curY - 1) && isDeadendPiece(curX, curY))
                {
                    availableDirections.Add(currentDirection * -1);
                } else
                {
                    availableDirections.Add(Vector2.left);
                    availableDirections.Add(Vector2.down);
                    availableDirections.Add(Vector2.up);
                    availableDirections.Remove(currentDirection * -1);
                }
            }
            if (colU && currentDirection.Equals(Vector2.up))
            {
                availableDirections.Clear();
                if (colU && isDeadendPiece(curX, curY) && isDeadendPiece(curX - 1, curY))
                {
                    availableDirections.Add(currentDirection * -1);
                } else
                {
                    availableDirections.Add(Vector2.left);
                    availableDirections.Add(Vector2.right);
                    availableDirections.Add(Vector2.down);
                    availableDirections.Remove(currentDirection * -1);
                }
            }
            if (colD && currentDirection.Equals(Vector2.down))
            {
                availableDirections.Clear();
                if (colD && isDeadendPiece(curX - 1, curY - 1) && isDeadendPiece(curX, curY - 1))
                {
                    availableDirections.Add(currentDirection * -1);
                } else
                {
                    availableDirections.Add(Vector2.left);
                    availableDirections.Add(Vector2.right);
                    availableDirections.Add(Vector2.up);
                    availableDirections.Remove(currentDirection * -1);
                }
            }
            lockoutTimer = lockoutTime;
        }
        moveWitness(availableDirections[Random.Range(0, availableDirections.Count)]);
        lockoutTimer -= Time.deltaTime;
    }

    private void moveWitness(Vector2 direction)
    {
        movement = direction;
        if((movement.y > 0 && colU )||(movement.y < 0 && colD))
        {
            movement.y = 0;
        }
        if((movement.x > 0 && colR)||(movement.x < 0 && colL))
        {
            movement.x = 0;
        }
        transform.position += (Vector3) movement.normalized * speed;

        curX = Mathf.RoundToInt(transform.position.x);
        curY = Mathf.RoundToInt(transform.position.y);
        currentDirection = direction;
    }

    private void checkCollisions()
    {
        colL = raycastHit(Vector2.left);
        colR = raycastHit(Vector2.right);
        colU = raycastHit(Vector2.up);
        colD = raycastHit(Vector2.down);
    }

    private bool raycastHit(Vector2 direction)
    {
        RaycastHit2D[] hits = new RaycastHit2D[1];
        return 0 != Physics2D.Raycast(transform.position, direction, contactFilter, hits, collisionDetectionLength);
    }

    private bool isCorner(int x, int y)
    {
        Sprite sprite = maze.GetSprite(new Vector3Int(x, y, 0));
        if (sprite == null || sprite.Equals(null))
        {
            return false;
        }
        string spriteName = sprite.name;
        return spriteName.EndsWith("_0") || spriteName.EndsWith("_2") || spriteName.EndsWith("_8") || spriteName.EndsWith("_10");
    }

    private bool isDeadendPiece(int x, int y)
    {
        Sprite sprite = maze.GetSprite(new Vector3Int(x, y, 0));
        if(sprite.Equals(null))
        {
            return false;
        }
        string spriteName = sprite.name;
        return spriteName.EndsWith("_14") || spriteName.EndsWith("_15") || spriteName.EndsWith("_16") || spriteName.EndsWith("_17");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.left * collisionDetectionLength);
        Gizmos.DrawRay(transform.position, Vector2.right * collisionDetectionLength);
        Gizmos.DrawRay(transform.position, Vector2.up * collisionDetectionLength);
        Gizmos.DrawRay(transform.position, Vector2.down * collisionDetectionLength);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {
            var list = CafeSceneScript.instance.generateSandwich();
            var things = "";

            for (int i = 0; i < list.Length; i++)
            {
                things += list[i] + "  ";
            }

            elemets.text = things;
            isDone = true;
        }
    }
    void timerEnded()
    {
        SceneManager.LoadScene("MapSelect");
    }
}

