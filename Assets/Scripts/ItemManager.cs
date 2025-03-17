using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemsParent;

    public RSO_PlayerPosition playerPosition;
    public RSO_ItemsLeft itemsLeft;
    public RSO_MapDefinition mapDefinition;

    private Dictionary<Vector2Int, GameObject> items = new Dictionary<Vector2Int, GameObject>();

    private void OnEnable()
    {
        mapDefinition.onValueChanged += Initialize;
        playerPosition.onValueChanged += HandlePlayerPositionChange;
    }

    private void OnDisable()
    {
        mapDefinition.onValueChanged -= Initialize;
        playerPosition.onValueChanged -= HandlePlayerPositionChange;
    }


    private void Awake()
    {
        itemsLeft.Value = 0;
    }

    private void AddItem(Vector2Int position, GameObject item)
    {
        items.Add(position, item);
        itemsLeft.Value++;
    }

    private void HandlePlayerPositionChange(Vector2Int position)
    {
        if (items.ContainsKey(position))
        {
            Destroy(items[position]);
            items.Remove(position);
            itemsLeft.Value--;
        }
    }

    private void Initialize(MapDefinition mapDefinition)
    {
        GenerateItems(mapDefinition.itemsLayer, itemPrefab, 1);
    }

    private void GenerateItems(int[] layer, GameObject tilePrefab, int tileType)
    {
        for (int x = 0; x < mapDefinition.Value.width; x++)
        {
            for (int y = 0; y < mapDefinition.Value.height; y++)
            {
                if (layer[x * mapDefinition.Value.width + y] == tileType)
                {
                    GameObject item = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity, itemsParent);
                    AddItem(new Vector2Int(x, y), item);
                }
            }
        }
    }
}
