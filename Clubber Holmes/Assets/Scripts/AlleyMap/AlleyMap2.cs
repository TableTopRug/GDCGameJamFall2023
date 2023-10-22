using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine.Tilemaps;
using UnityEngine;


public class AlleyMap2 : MonoBehaviour
{
    public int width, height;
    public bool[,] tiles;

    // Player starting position.
    public int startingX, startingY;
    // Witness starting position.
    public int endingX, endingY;

    // Tilemap
    public Tilemap tilemap;

    // Tiles
    public RuleTile buildingTile;
    public RuleTile pathTile;

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
            if (Mathf.Abs(endingX-startingX >= minDiff)) { break; }
            if (Mathf.Abs(endingY-startingY >= minDiff)) { break; }
        }

        // Fill alley with a specific tile.
        tilemap.BoxFill(new Vector3Int(0,0,0), buildingTile, startingX, startingY, endingX, endingY);


        // For all spaces between grid, put a tile.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tiles[x,y])
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), pathTile);
                }
            }
        }
    }


    public bool[,] GenerateAlley(int w, int h)
    {
        bool[,] grid = new bool[w, h];

        bool[,] visited = new bool[w, h];

        bool dfs(int x, int y)
        {
            if (visited[x, y])
            {
                return false;
            }
            visited[x, y] = true;

            var dirs = new[]
            {
                (x-1, y, grid, x, y),
                (x+1, y, grid, x, y),
                (x, y-1, grid, x, y),
                (x, y+1, grid, x, y),
            };

            foreach (var (nx, ny, g, wx, wy) in dirs.OrderBy(t => frand()))
            {
                grid[wx, wy] = !(0 <= nx && nx < w && 0 <= ny && ny < h && (dfs(nx, ny) || frand() < WitnessProbability));
            }

            return true;
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
