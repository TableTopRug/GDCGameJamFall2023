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
    private bool colL, colR, colU, colD;
    private bool cornerL, cornerR, cornerU, cornerD;

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
        if(colL)
        {
            if(isCorner(curX + 1, curY + 1))
            {
                //can go up
            }
            if (isCorner(curX + 1, curY - 1))
            {
                //can go down
            }
        }
        if(colR)
        {
            if (isCorner(curX - 1, curY + 1))
            {
                //can go up
            }
            if (isCorner(curX - 1, curY - 1))
            {
                //can go down
            }
        }
        if(colU)
        {
            if (isCorner(curX + 1, curY - 1))
            {
                //can go right
            }
            if (isCorner(curX - 1, curY - 1))
            {
                //can go left
            }
        }
        if(colD)
        {
            if (isCorner(curX + 1, curY + 1))
            {
                //can go right
            }
            if (isCorner(curX - 1, curY + 1))
            {
                //can go left
            }
        }
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
