using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPowerUpManager : MonoBehaviour
{
    [SerializeField] private float timeGhostMode = 5f;
    [SerializeField] private GameObject ghostPowerUpPrefab;
    [SerializeField] private Transform ghostPowerUpsParent;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private ItemManager itemManager;

    public RSE_OnPlayerFinishLevel onPlayerFinishLevel;
    public RSO_PlayerPosition playerPosition;
    public RSO_MapDefinition mapDefinition;
    public RSO_PlayerGhostMode playerGhostMode;
    public RSE_OnPlayerDie onPlayerDie;
    public Dictionary<Vector2Int, GameObject> ghostPowerUps = new Dictionary<Vector2Int, GameObject>();

    private Coroutine ghostModeCoroutine;

    private void Awake()
    {
        playerGhostMode.Value = false;
    }
    private void OnEnable()
    {
        mapDefinition.onValueChanged += Initialize;
        playerPosition.onValueChanged += HandlePlayerPositionChange;
        onPlayerFinishLevel.action += StopGhostMode;
    }

    private void OnDisable()
    {
        mapDefinition.onValueChanged -= Initialize;
        playerPosition.onValueChanged -= HandlePlayerPositionChange;
        onPlayerFinishLevel.action -= StopGhostMode;
    }

    private void AddItem(Vector2Int position, GameObject item)
    {
        ghostPowerUps.Add(position, item);
    }

    private void HandlePlayerPositionChange(Vector2Int position)
    {
        if (ghostPowerUps.ContainsKey(position))
        {
            Destroy(ghostPowerUps[position]);
            ghostPowerUps.Remove(position);

            if(ghostModeCoroutine != null)
                StopCoroutine(ghostModeCoroutine);

            ghostModeCoroutine = StartCoroutine(ActiveGhostMode());
        }
    }

    void StopGhostMode()
    {
        if (ghostModeCoroutine != null)
            StopCoroutine(ghostModeCoroutine);

        playerGhostMode.Value = false;
    }

    private IEnumerator ActiveGhostMode()
    {
        playerGhostMode.Value = true;

        yield return new WaitForSeconds(timeGhostMode);

        playerGhostMode.Value = false;

        if (IsWall(playerPosition.Value))
        {
            Debug.Log("Walled");
            onPlayerDie.RaiseEvent();
        }
    }
    private bool IsWall(Vector2Int position)
    {
        if (position.x < 0 || position.x >= mapDefinition.Value.width || position.y < 0 || position.y >= mapDefinition.Value.height)
            return true;

        return mapDefinition.Value.wallLayer[position.x * mapDefinition.Value.width + position.y] == 1;
    }

    private void Initialize(MapDefinition mapDefinition)
    {
        ghostPowerUps.Clear();
        GenerateItems(mapDefinition.ghostPowerUpLayer, ghostPowerUpPrefab, 1);
    }

    private void GenerateItems(int[] layer, GameObject tilePrefab, int tileType)
    {
        for (int i = 0; i < 1; i++)
        {
            bool isValid = false;
            Vector2Int spawnpoint = Vector2Int.zero;

            while (!isValid)
            {
                spawnpoint = new Vector2Int(Random.Range(0, levelManager._mapDefinition.width), Random.Range(0, levelManager._mapDefinition.height));

                if (!IsWall(spawnpoint)
                    && !ghostPowerUps.ContainsKey(spawnpoint)
                    && !itemManager.items.ContainsKey(spawnpoint)
                    && new Vector3(spawnpoint.x, 0, spawnpoint.y) != new Vector3(levelManager._mapDefinition.exitCoordinates[0], 0, levelManager._mapDefinition.exitCoordinates[1])
                    && new Vector3(spawnpoint.x, 0, spawnpoint.y) != new Vector3(levelManager._mapDefinition.spawnCoordinates[0], 0, levelManager._mapDefinition.spawnCoordinates[1]))
                {
                    isValid = true;
                }
            }

            GameObject item = Instantiate(tilePrefab, new Vector3(spawnpoint.x, 0, spawnpoint.y), Quaternion.identity, ghostPowerUpsParent);
            AddItem(new Vector2Int(spawnpoint.x, spawnpoint.y), item);
        }
    }
}
