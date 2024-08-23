using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorsOfTheEnemies
{
    // Initialize the list with the correct enum references
    public static List<ColorEnemy> colors = new List<ColorEnemy>
    {
        ColorEnemy.Green,
        ColorEnemy.Purple,
        ColorEnemy.Red
    };
    public static ColorEnemy GetRandomColor()
    {
        return colors[UnityEngine.Random.Range(0,colors.Count)];
    }
}
public enum ColorEnemy
{
    None,
    Green,
    Red,
    Purple
}
