using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level
{
    public int Rows;
    public int Columns;
    public float TileSize;

    public List<Layer> Layers;

    public void UpdateLevel(int rows, int columns, float tileSize)
    {
        Rows = rows;
        Columns = columns;
        TileSize = tileSize;
    }

    public Level(int rows, int columns, float tileSize)
    {
        Rows = rows;
        Columns = columns;
        TileSize = tileSize;
        Layers = new List<Layer>() { new Layer("Entities", 0, null), new Layer("Tiles", 1, null), new Layer("Scenery", 2, null) };
    }
}
