using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine.Tilemaps;
using UnityEngine;

/*
 * Generates the Alley Map.
 * 
 */
public class AlleyMap2 : MonoBehaviour
{
    // Width and height of the tilemap.
    /*
     * Movement amount says how much does the DFS algorithm jumps between coordinates.
     * Movement cannot be 1 or the DFS is going to set most of the grid as false.
     */ 
    public int width, height, movementAmount;
    // Holds whether or not there should be tiles.
    public bool[,] tiles;

    // Player starting position.
    public int startingX, startingY;
    // Witness starting position.
    public int endingX, endingY;

    // Tilemap
    public Tilemap tilemap;

    // Tiles
    public RuleTile buildingTile;


    public float WitnessProbability;

    // Start is called before the first frame update
    void Start()
    {
        // Clear the tiles from the alley.
        clearAlley();

        // Generate a grid with the connected path.
        tiles = GenerateAlley(width, height);

        // Generate random coordinates where the player starts.
        startingX = Rand(width);
        startingY = Rand(height);

        // Get a minimum difference so that the end doesn't spawn next to the start.
        int minDiff = Mathf.Max(width, height) / 2;

        /*
         * While true, then:
         * 1) Generate random coordinates for the end.
         * 2) If the difference between the start and end is greater than the minimum difference, then break.
         */
        while (true)
        {
            endingX = Rand(width);
            endingY = Rand(height);
            if (Mathf.Abs(endingX - startingX) >= minDiff) { break; }
            if (Mathf.Abs(endingY - startingY) >= minDiff) { break; }
        }

        // For all coordinates in grid, put a tile.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), buildingTile);
            }
        }

        // For all coordinates in grid, remove tile if the tile's coordinate is false.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tiles[x,y])
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
        }

        // *** I don't remember why I added this. ***
        // For all false coordinates in grid, put a tile if it has a tile next to it. 
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == startingX || y == startingY || x == endingX || y == endingY)
                {
                    continue;
                }

                if (!tilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    if (tilemap.HasTile(new Vector3Int(x + 1, y, 0)))
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), buildingTile);
                    }

                    if (tilemap.HasTile(new Vector3Int(x - 1, y, 0)))
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), buildingTile);
                    }

                    if (tilemap.HasTile(new Vector3Int(x, y + 1, 0)))
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), buildingTile);
                    }

                    if (tilemap.HasTile(new Vector3Int(x, y - 1, 0)))
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), buildingTile);
                    }
                }
            }
        }

        // *** I don't remember why I added this. ***
        // For all spaces between grid, remove tile if it has no tiles next to it. 
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!tilemap.HasTile(new Vector3Int(x - 1, y, 0)) && !tilemap.HasTile(new Vector3Int(x + 1, y, 0)))
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), null);
                }

                if (!tilemap.HasTile(new Vector3Int(x, y - 1, 0)) && !tilemap.HasTile(new Vector3Int(x, y + 1, 0)))
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
        }
    }

    /*
     * Generates alley "room" with corners prepared
     * Returns a grid that contains the coordinates with tile and with no tile.
     */
    public bool[,] GenerateAlley(int w, int h)
    {
        // Size of the alley including borders.
        bool[,] grid = new bool[w+2, h+2];
        
        // Keeps track of the the visited coordinates.
        bool[,] visited = new bool[w, h];

        /*
         * Recursive Depth First Search algorithm to:
         * 1) Create a path until you hit the corners or has been visited.
         */
        bool dfs(int x, int y)
        {
            if (x >= w ||  y >= h)
            {
                return false;
            }
            if (visited[x, y])
            {
                return false;
            }
            visited[x, y] = true;

            var dirs = new[]
            {
                (x-movementAmount, y, grid, x, y),
                (x+movementAmount, y, grid, x, y),
                (x, y-movementAmount, grid, x, y),
                (x, y+movementAmount, grid, x, y),
            };

            // *** I think this is a calculation from online. To make it more random or something. ***
            foreach (var (nx, ny, g, wx, wy) in dirs.OrderBy(t => frand()))
            {
                grid[wx, wy] = !(0 <= nx && nx < w && 0 <= ny && ny < h && (dfs(nx, ny) || frand() < WitnessProbability));
            }

            return true;
        }

        // Fill corner tiles.
        for (int i = 0; i < width; i++ )
        {
            tilemap.SetTile(new Vector3Int(i, -1, 0), buildingTile);
            tilemap.SetTile(new Vector3Int(i, height, 0), buildingTile);

            tilemap.SetTile(new Vector3Int(-1, i, 0), buildingTile);
            tilemap.SetTile(new Vector3Int(width, i, 0), buildingTile);
        }

        // Start DFS algorithm.
        dfs(0, 0);

        return grid;
    } 

    // Return random integer.
    private int Rand(int max)
    {
        return UnityEngine.Random.Range(0, max);
    }

    // Return random float.
    private float frand()
    {
        return UnityEngine.Random.value;
    }

    // Clear all the tiles related to the tilemap.
    private void clearAlley()
    {
        // Clear all tiles
        tilemap.ClearAllTiles();
    }
}

/***
 * ADDITIONAL SUGGESTIONS:
 * 1) I think that the DFS algorithm needs to be changed.
 * 2) From lines 88-140, I think I was trying to make sure the maze didn't have holes and missing texture.
 * 3) I think the clear alley was a way to reset the alley generation.
 * 4) Code from lines 66-85 could be merged.
 ***/