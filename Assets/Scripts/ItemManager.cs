using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public RSO_PlayerPosition playerPosition;
    public RSO_MapDefinition mapDefinition;

    private Dictionary<Vector2Int, bool> items;

    private void OnEnable()
    {
        mapDefinition.onValueChanged += Initialize;
    }

    private void OnDisable()
    {
        mapDefinition.onValueChanged -= Initialize;
    }

    // WIP
    private void Initialize(MapDefinition mapDefinition)
    {
        items = new Dictionary<Vector2Int, bool>();

        for (int i = 0; i < mapDefinition.height; i++)
        {
            for (int j = 0; j < mapDefinition.width; j++)
            {
                if (mapDefinition.itemsLayer[i * mapDefinition.height + j] == 1)
                {
                    items.Add(new Vector2Int(i, j), true);
                }
            }
        }
    }

    
}
