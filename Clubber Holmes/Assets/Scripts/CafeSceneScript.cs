using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class CafeSceneScript : MonoBehaviour
{
    public static string[] ingrediants = {"White Bread", "Wheat Bread", "Turkey", "Chicken", "Bacon", "Lettuce", "Tomato", "Egg", "Mustard", "Mayonaise"};
    public List<string> curIngrediants = new List<string>();
    public List<string> selectedIngredients = new List<string>();


    public void generateSandwich()
    {
        var random = new System.Random();
        var bread = ingrediants[random.Next(0, 2)];
        var sause = ingrediants[random.Next(8, 10)];
        var mid1 = ingrediants[random.Next(2, 8)];
        var mid2 = ingrediants[random.Next(2, 8)];

        curIngrediants.Clear();
        selectedIngredients.Clear();
        while (mid2 == mid1)
        {
            mid2 = ingrediants[random.Next(2, 8)];
        }

        curIngrediants.Add(bread);
        curIngrediants.Add(mid1);
        curIngrediants.Add(mid2);
        curIngrediants.Add(sause);

        Console.WriteLine(curIngrediants.ToArray());
    }

    public bool checkSandwich()
    {
        if (curIngrediants.Count == selectedIngredients.Count)
        {
            bool[] correct = new bool[4];
            for (int i = 0; i < curIngrediants.Count; i++)
            {
                correct[i] = curIngrediants.Contains(selectedIngredients[i]);
            }

            return correct[0] && correct[1] && correct[2] && correct[3];
        }

        return false;
    }

    public void updateIngrediants(string ing)
    {
        if (curIngrediants.Contains(ing))
        {
            selectedIngredients.Remove(ing);
        }
        else
        {
            selectedIngredients.Add(ing);
        }
    }

    public void submitOrder()
    {
        if (checkSandwich())
        {
            generateNote(true);
        } else
        {
            generateNote(false);
        }
    }

    public void generateNote(bool correct)
    {
        Console.WriteLine("Club!");
    }
}
