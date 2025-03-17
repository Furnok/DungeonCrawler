using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPowerUpManager : MonoBehaviour
{
    [SerializeField] private float timeGhostMode = 5f;
    [SerializeField] private GameObject ghostPowerUpPrefab;
    [SerializeField] private Transform ghostPowerUpsParent;

    public RSO_PlayerPosition playerPosition;
    public RSO_MapDefinition mapDefinition;
    public RSO_PlayerGhostMode playerGhostMode;
    public RSE_OnPlayerDie onPlayerDie;
    private Dictionary<Vector2Int, GameObject> ghostPowerUps = new Dictionary<Vector2Int, GameObject>();

    private Coroutine ghostModeCoroutine;

    private void Awake()
    {
        playerGhostMode.Value = false;
    }
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
        GenerateItems(mapDefinition.ghostPowerUpLayer, ghostPowerUpPrefab, 1);
    }

    private void GenerateItems(int[] layer, GameObject tilePrefab, int tileType)
    {
        for (int x = 0; x < mapDefinition.Value.width; x++)
        {
            for (int y = 0; y < mapDefinition.Value.height; y++)
            {
                if (layer[x * mapDefinition.Value.width + y] == tileType)
                {
                    GameObject item = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity, ghostPowerUpsParent);
                    AddItem(new Vector2Int(x, y), item);
                }
            }
        }
    }
}
