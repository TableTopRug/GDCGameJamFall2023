using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlleyMap : MonoBehaviour
{
    public int width, height;

    public bool[,] verticalBuildings, horizontalBuildings;
    
    // Player starting position
    public int startingX, startingY;
    public int endingX, endingY;

    // Transform
    public Transform Buildings;

    // Templates
    public GameObject Building;

    public float WitnessProbability;

    // Start is called before the first frame update
    void Start()
    {
        // Clean buildings from map.
        cleanBuildings();

        (horizontalBuildings, verticalBuildings) = GenerateAlley(width, height);
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

        for (int x = 0; x < width + 1; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (horizontalBuildings[x, y])
                {
                    Instantiate(Building, new Vector3(x, y + 0.5f, 0), Quaternion.Euler(0, 0, 90), Buildings);
                }
            }
        }

        for (int x = 0; x < width + 1; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (verticalBuildings[x, y])
                {
                    Instantiate(Building, new Vector3(x + 0.5f, y, 0), Quaternion.identity, Buildings);
                }
            }
        }
    }

    // Return random number.
    public int Rand(int max)
    {
        return UnityEngine.Random.Range(0, max);
    }

    public float frand()
    {
        return UnityEngine.Random.value;
    }

    public (bool[,], bool[,]) GenerateAlley(int w, int h)
    {
        bool[,] hbuildings = new bool[w, h];
        bool[,] vbuildings = new bool[w, h];

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
                (x - 1, y, hbuildings, x, y),
                (x + 1, y, hbuildings, x + 1, y),
                (x, y - 1, vbuildings, x, y),
                (x, y + 1, vbuildings, x, y + 1),
            };

            foreach (var (nx, ny, building, wx, wy) in dirs.OrderBy(t => frand()))
                building[wx, wy] = !(0 <= nx && nx < w && 0 <= ny && ny < h && (dfs(nx, ny) || frand() < WitnessProbability));

            return true;
        }
        dfs(0, 0);

        return (hbuildings, vbuildings);
    }

    // Switches the current alley.
    public void SwitchAlley()
    {
        
    }

    private void cleanBuildings()
    {
        // Destroy current walls.
        foreach (Transform child in Buildings)
        {
            Destroy(child.gameObject);
        }
    }
}
