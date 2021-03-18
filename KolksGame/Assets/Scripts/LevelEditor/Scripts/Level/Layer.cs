using System;
using System.Collections.Generic;

[Serializable]
public class Layer
{
    public string LayerName;
    public int LayerId;
    public List<Tile> Tiles;

    public Layer(string layerName, int layerId, List<Tile> tiles)
    {
        LayerName = layerName;
        LayerId = layerId;
        Tiles = tiles;
    }
}
