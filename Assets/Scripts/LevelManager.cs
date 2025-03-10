using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TextAsset mapDefinitionJSON;
    [SerializeField] private GameObject wallTile;
    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject exitSign;


    private MapDefinition _mapDefinition;
    private List<GameObject> walls, floors;
    public RSO_PlayerPosition playerPosition;
    public RSO_MapDefinition mapDefinition;

    private void Start()
    {
        ParseJSON();
        GenerateDungeon();
        SpawnPlayer();
    }

    private void ParseJSON()
    {
        _mapDefinition = JsonUtility.FromJson<MapDefinition>(mapDefinitionJSON.text);
        _mapDefinition.RotateMapClockWise();
        mapDefinition.Value = _mapDefinition;
    }

    private void SpawnPlayer()
    {
        playerPosition.Value = new Vector2Int(_mapDefinition.spawnCoordinates[0], _mapDefinition.spawnCoordinates[1]);
    }

    private void GenerateDungeon()
    {
        GenerateWalls();
        GenerateFloors();
        GenerateExit();
    }

    private void GenerateWalls()
    {
        GenerateTiles(_mapDefinition.wallLayer, wallTile, 1);
    }

    private void GenerateFloors()
    {
        GenerateTiles(_mapDefinition.wallLayer, floorTile, 0);
    }

    private void GenerateTiles(int[] layer, GameObject tilePrefab, int tileType)
    {
        for (int x = 0; x < _mapDefinition.width; x++)
        {
            for (int y = 0; y < _mapDefinition.height; y++)
            {
                if (layer[x * _mapDefinition.width + y] == tileType)
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
        if (position.x < 0 || position.x >= _mapDefinition.width || position.y < 0 || position.y >= _mapDefinition.height)
            return true;

        return _mapDefinition.wallLayer[position.x * _mapDefinition.width + position.y] == 1;
    }

    private void GenerateExit()
    {
        Instantiate(exitSign, new Vector3(_mapDefinition.exitCoordinates[0], 0, _mapDefinition.exitCoordinates[1]), Quaternion.identity);
    }
}