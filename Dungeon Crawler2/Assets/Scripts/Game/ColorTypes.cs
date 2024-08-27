using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helping class for generating random colors for enemies
/// </summary>
public static class ColorsOfTheEnemies
{
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
/// <summary>
/// The enum of the 3 enemy types in the game
/// </summary>
public enum ColorEnemy
{
    None,
    Green,
    Red,
    Purple
}
