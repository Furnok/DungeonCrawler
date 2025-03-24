using System.Collections;
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

    public MapDefinition _mapDefinition;
    private List<GameObject> walls, floors;
    public RSO_PlayerPosition playerPosition;
    public RSO_MapDefinition mapDefinition;
    public RSO_PlayerGhostMode playerGhostMode;
    public RSE_OnPlayerFinishLevel onPlayerFinishLevel;
    public RSE_OnPlayerDie onPlayerDie;
    public RSO_PlayerIsMoving playerIsMoving;
    public RSO_LevelNumber _levelNumber;
    public RSE_OnPlayerMove onPlayerMove;

    private GameObject exitGameObject;

    private Vector2 startPos;
    private float minSwipeDistance = 50f; // Minimum distance to register a swipe

    private void Awake()
    {
        playerPosition.Value = Vector2Int.zero;
        _levelNumber.Value = 0;
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
        _levelNumber.Value++;
        if(mapDefinitionJSON.Count <= _levelNumber.Value)
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
        _mapDefinition = JsonUtility.FromJson<MapDefinition>(mapDefinitionJSON[_levelNumber.Value].text);
        _mapDefinition.RotateMapClockWise();
        mapDefinition.Value = _mapDefinition;
    }

    private void SpawnPlayer()
    {
        StartCoroutine(Setup());
    }

    IEnumerator Setup()
    {
        yield return new WaitForSeconds(0.1f);
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
            //TryMove(ctx.ReadValue<Vector2>());
        }
    }

    public void OnLeftMouseInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            startPos = Pointer.current.position.ReadValue();
        }
        else if (ctx.canceled)
        {
            Vector2 endPos = Pointer.current.position.ReadValue();
            DetectSwipe(startPos, endPos);
        }
    }

    private void DetectSwipe(Vector2 start, Vector2 end)
    {
        Vector2 swipeVector = end - start;

        if (swipeVector.magnitude < minSwipeDistance) return;

        if (Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y))
        {
            if (swipeVector.x > 0)
            {
                TryMove(new Vector2(1, 0));
            }
            else
            {
                TryMove(new Vector2(-1, 0));
            }
                
        }
        else
        {
            if (swipeVector.y > 0)
            {
                TryMove(new Vector2(0, 1));
            } 
            else
            {
                TryMove(new Vector2(0, -1));
            }      
        }
    }

    private void TryMove(Vector2 direction)
    {
        Vector2Int destination = playerPosition.Value + new Vector2Int((int)direction.x, (int)direction.y);

        if (playerGhostMode.Value && destination.x < _mapDefinition.width && destination.y < _mapDefinition.height)
        {
            playerPosition.Value = destination;
            onPlayerMove.RaiseEvent();
        }

        if (!IsWall(destination) && !playerIsMoving.Value && !playerIsMoving.Value)
        {
            playerPosition.Value = destination;
            onPlayerMove.RaiseEvent();
        }
    }

    public bool IsWall(Vector2Int position)
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