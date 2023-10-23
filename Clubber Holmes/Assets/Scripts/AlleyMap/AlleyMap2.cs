using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine.Tilemaps;
using UnityEngine;


public class AlleyMap2 : MonoBehaviour
{
    public int width, height, movementAmount;
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
        clearAlley();

        tiles = GenerateAlley(width, height);

        startingX = Rand(width);
        startingY = Rand(height);

        int minDiff = Mathf.Max(width, height) / 2;

        while (true)
        {
            endingX = Rand(width);
            endingY = Rand(height);
            if (Mathf.Abs(endingX - startingX) >= minDiff) { break; }
            if (Mathf.Abs(endingY - startingY) >= minDiff) { break; }
        }

        // For all space in grid, put a tile.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), buildingTile);
            }
        }

        // For all spaces between grid, remove tile depending on where there is no tile.
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

        // For all space in grid, put a tile if it has a tile nearby.
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

        // For all spaces between grid, remove tile depending on where there is no tile.
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


    public bool[,] GenerateAlley(int w, int h)
    {
        bool[,] grid = new bool[w+2, h+2];

        bool[,] visited = new bool[w, h];

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

            foreach (var (nx, ny, g, wx, wy) in dirs.OrderBy(t => frand()))
            {
                grid[wx, wy] = !(0 <= nx && nx < w && 0 <= ny && ny < h && (dfs(nx, ny) || frand() < WitnessProbability));
            }

            return true;
        }

        for (int i = 0; i < width; i++ )
        {
            tilemap.SetTile(new Vector3Int(i, -1, 0), buildingTile);
            tilemap.SetTile(new Vector3Int(i, height, 0), buildingTile);

            tilemap.SetTile(new Vector3Int(-1, i, 0), buildingTile);
            tilemap.SetTile(new Vector3Int(width, i, 0), buildingTile);
        }


        dfs(0, 0);

        return grid;
    } 



    // Return random number.
    private int Rand(int max)
    {
        return UnityEngine.Random.Range(0, max);
    }

    private float frand()
    {
        return UnityEngine.Random.value;
    }

    private void clearAlley()
    {
        // Clear all tiles
        tilemap.ClearAllTiles();
    }
}
