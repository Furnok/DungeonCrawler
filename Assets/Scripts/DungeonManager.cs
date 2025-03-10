using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] private TextAsset mapDefinitionJSON;
    [SerializeField] private GameObject wallTile;
    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject exitSign;
    [SerializeField] private GameObject itemPrefab;

    private MapDefinition mapDefinition;
    private List<GameObject> walls, floors;
    public RSO_PlayerPosition playerPosition;
    public RSO_ExitPosition exitPosition;

    private void Start()
    {
        ParseJSON();
        GenerateDungeon();
        SpawnPlayer();
    }

    private void ParseJSON()
    {
        mapDefinition = JsonUtility.FromJson<MapDefinition>(mapDefinitionJSON.text);
        mapDefinition.RotateMapClockWise();
    }

    private void SpawnPlayer()
    {
        playerPosition.Value = new Vector2Int(mapDefinition.spawnCoordinates[0], mapDefinition.spawnCoordinates[1]);
    }

    private void GenerateDungeon()
    {
        GenerateWalls();
        GenerateFloors();
        GenerateExit();
        GenerateItems();
    }

    private void GenerateWalls()
    {
        GenerateTiles(mapDefinition.wallLayer, wallTile, 1);
    }

    private void GenerateFloors()
    {
        GenerateTiles(mapDefinition.wallLayer, floorTile, 0);
    }

    private void GenerateTiles(int[] layer, GameObject tilePrefab, int tileType)
    {
        for (int x = 0; x < mapDefinition.width; x++)
        {
            for (int y = 0; y < mapDefinition.height; y++)
            {
                if (layer[x * mapDefinition.width + y] == tileType)
                {
                    Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity);
                }
            }
        }
    }

    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            TryMove(ctx.ReadValue<Vector2>());
        }
    }

    private void TryMove(Vector2 direction)
    {
        Vector2Int destination = playerPosition.Value + new Vector2Int((int)direction.x, (int)direction.y);

        if (!IsWall(destination))
            playerPosition.Value = destination;
    }

    private bool IsWall(Vector2Int position)
    {
        if (position.x < 0 || position.x >= mapDefinition.width || position.y < 0 || position.y >= mapDefinition.height)
            return true;

        return mapDefinition.wallLayer[position.x * mapDefinition.width + position.y] == 1;
    }

    private void GenerateExit()
    {
        Instantiate(exitSign, new Vector3(mapDefinition.exitCoordinates[0], 0, mapDefinition.exitCoordinates[1]), Quaternion.identity);
    }

    private void GenerateItems()
    {
        GenerateTiles(mapDefinition.itemsLayer, itemPrefab, 1);
    }
}
