using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WitnessAI : MonoBehaviour
{
    [SerializeField] private Tilemap maze;
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private float collisionDetectionLength;
    private int curX, curY;
    public bool colL, colR, colU, colD;
    private bool canMoveL, canMoveR, canMoveU, canMoveD;
    private Vector2 movement;
    private Vector2 currentDirection;

    // Start is called before the first frame update
    void Start()
    {
        moveWitness(Vector2.up);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //[L, R, U, D]
        List<Vector2> availableDirections = new List<Vector2>();
        if(colL)
        {
            if(isCorner(curX + 1, curY + 1))
            {
                availableDirections.Add(Vector2.up);
            }
            if (isCorner(curX + 1, curY - 1))
            {
                availableDirections.Add(Vector2.down);
            }
        }
        if(colR)
        {
            if (isCorner(curX - 1, curY + 1))
            {
                availableDirections.Add(Vector2.up);
            }
            if (isCorner(curX - 1, curY - 1))
            {
                availableDirections.Add(Vector2.down);
            }
        }
        if(colU)
        {
            if (isCorner(curX + 1, curY - 1))
            {
                availableDirections.Add(Vector2.right);
            }
            if (isCorner(curX - 1, curY - 1))
            {
                availableDirections.Add(Vector2.left);
            }
        }
        if(colD)
        {
            if (isCorner(curX + 1, curY + 1))
            {
                availableDirections.Add(Vector2.right);
            }
            if (isCorner(curX - 1, curY + 1))
            {
                availableDirections.Add(Vector2.left);
            }
        }
        /*for(int i = 0; i< availableDirections.Count; i++)
        {
            if (availableDirections[i].Equals(currentDirection))
            {
                availableDirections.RemoveAt(i);
                i--;
            }
        }*/
        moveWitness(availableDirections[Random.Range(0, availableDirections.Count)]);
    }

    private int directionToIntCode(Vector2 direction)
    {
        if(direction.Equals(Vector2.left))
        {
            return 0;
        } else if(direction.Equals(Vector2.right))
        {
            return 1;
        } else if(direction.Equals(Vector2.up))
        {
            return 2;
        } else if(direction.Equals(Vector2.down))
        {
            return 3;
        }
        return -1;
    }

    private void moveWitness(Vector2 direction)
    {
        movement = (Vector2) transform.position + direction;
        transform.position = movement;
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
        RaycastHit2D? hit = Physics2D.Raycast(transform.position, direction, collisionDetectionLength, terrainLayer);
        return hit != null;
    }

    private bool isCorner(int x, int y)
    {
        string spriteName = maze.GetSprite(new Vector3Int(x, y, 0)).name;
        return spriteName.EndsWith("_0") || spriteName.EndsWith("_2") || spriteName.EndsWith("_8") || spriteName.EndsWith("_10");
    }
}
