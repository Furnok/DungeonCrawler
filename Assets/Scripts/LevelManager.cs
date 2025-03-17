using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<TextAsset> mapDefinitionJSON;
    [SerializeField] private GameObject wallTile;
    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject exitSign;
    [SerializeField] private Transform wallsParent;
    [SerializeField] private Transform floorsParent;
    [SerializeField] private Transform levelParent;
    [SerializeField] private GameObject[] allLevel;

    private MapDefinition _mapDefinition;
    private List<GameObject> walls, floors;
    public RSO_PlayerPosition playerPosition;
    public RSO_MapDefinition mapDefinition;
    public RSO_PlayerGhostMode playerGhostMode;
    public RSE_OnPlayerFinishLevel onPlayerFinishLevel;
    public RSE_OnPlayerDie onPlayerDie;

    private int levelNumber = 0;

    GameObject exitGameObject;

    private void Awake()
    {
        playerPosition.Value = Vector2Int.zero;
    }

    private void Start()
    {
        ParseJSON();
        GenerateDungeon();
        SpawnPlayer();
    }

    private void OnEnable()
    {
        onPlayerDie.action += LoadMenu;
        onPlayerFinishLevel.action += GenerateGame;
    }

    private void OnDisable()
    {
        onPlayerDie.action -= LoadMenu;
        onPlayerFinishLevel.action -= GenerateGame;
    }

    void GenerateGame()
    {
        levelNumber++;
        if(mapDefinitionJSON.Count <= levelNumber)
        {
            LoadMenu();
            return;
        }
        ClearLevel();
        ParseJSON();
        GenerateDungeon();
        SpawnPlayer();
    }

    private void ParseJSON()
    {
        _mapDefinition = JsonUtility.FromJson<MapDefinition>(mapDefinitionJSON[levelNumber].text);
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

    private void ClearLevel()
    {
        foreach (GameObject level in allLevel)
        {
            foreach (Transform child in level.transform)
            {
                Destroy(child.gameObject);
            }
        }

        Destroy(exitGameObject);

    }

    private void GenerateWalls()
    {
        GenerateTiles(_mapDefinition.wallLayer, wallTile, 1, wallsParent);
    }

    private void GenerateFloors()
    {
        GenerateTiles(_mapDefinition.wallLayer, floorTile, 0, floorsParent);
    }

    private void GenerateTiles(int[] layer, GameObject tilePrefab, int tileType, Transform parent)
    {
        for (int x = 0; x < _mapDefinition.width; x++)
        {
            for (int y = 0; y < _mapDefinition.height; y++)
            {
                if (layer[x * _mapDefinition.width + y] == tileType)
                {
                    Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity, parent);
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

        if (!IsWall(destination) || playerGhostMode.Value == true)
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
        exitGameObject = Instantiate(exitSign, new Vector3(_mapDefinition.exitCoordinates[0], 0, _mapDefinition.exitCoordinates[1]), Quaternion.identity, levelParent);
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene("Scene_Menu");
    }
}