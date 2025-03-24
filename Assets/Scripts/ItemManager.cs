using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemsParent;
    [SerializeField] private int numberItems;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GhostPowerUpManager ghostPowerUpManager;

    public RSO_PlayerPosition playerPosition;
    public RSO_ItemsLeft itemsLeft;
    public RSO_MapDefinition mapDefinition;

    public Dictionary<Vector2Int, GameObject> items = new Dictionary<Vector2Int, GameObject>();

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
        itemsLeft.Value = 0;
        items.Clear();
        GenerateItems(mapDefinition.itemsLayer, itemPrefab, 1);
    }

    private void GenerateItems(int[] layer, GameObject tilePrefab, int tileType)
    {
        for (int i = 0; i < numberItems; i++)
        {
            bool isValid = false;
            Vector2Int spawnpoint = Vector2Int.zero;

            while (!isValid)
            {
                spawnpoint = new Vector2Int(Random.Range(0, levelManager._mapDefinition.width), Random.Range(0, levelManager._mapDefinition.height));

                if (!levelManager.IsWall(spawnpoint) 
                    && !ghostPowerUpManager.ghostPowerUps.ContainsKey(spawnpoint)
                    && !items.ContainsKey(spawnpoint)
                    && new Vector3(spawnpoint.x, 0, spawnpoint.y) != new Vector3(levelManager._mapDefinition.exitCoordinates[0], 0, levelManager._mapDefinition.exitCoordinates[1])
                    && new Vector3(spawnpoint.x, 0, spawnpoint.y) != new Vector3(levelManager._mapDefinition.spawnCoordinates[0], 0, levelManager._mapDefinition.spawnCoordinates[1]))
                {
                    isValid = true;
                }
            }

            GameObject item = Instantiate(tilePrefab, new Vector3(spawnpoint.x, 0, spawnpoint.y), Quaternion.identity, itemsParent);
            AddItem(new Vector2Int(spawnpoint.x, spawnpoint.y), item);
        }
    }
}
