using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestAlleyMap : MonoBehaviour
{
    public int width, height, movementAmount;
    public bool[,] tiles;

    // Player starting position.
    public int playerStartX, playerStartY;
    // Witness starting position.
    public int endingX, endingY;

    // Tilemap
    public Tilemap tilemap;

    // Tiles
    public RuleTile buildingTile;

    public float WitnessProbability;

    private int[,] travelMap;


    // Start is called before the first frame update
    void Start()
    {
        clearAlley();

        tiles = GenerateAlley(width, height);

        startingX = Rand(width);
        startingY = Rand(height);
    }

    public bool[,] GenerateAlley(int w, int h)
    {
        bool[,] grid = new bool[w + 2, h + 2];

        bool[,] visited = new bool[w, h];

        bool dfs(int x, int y)
        {
            if (x >= w || y >= h)
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

        for (int i = 0; i < width; i++)
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

internal class GridPos
{
    public int x, y;

    GridPos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
