using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    public RSO_PlayerPosition playerPosition;
    public RSE_OnPlayerFinishLevel onPlayerFinishLevel;
    public RSO_ItemsLeft itemsLeft;
    public RSO_MapDefinition mapDefinition;
    [SerializeField] private RSO_Level rsoLevel;

    private void OnEnable()
    {
        playerPosition.onValueChanged += CheckPlayerOnExitTile;
    }

    private void OnDisable()
    {
        playerPosition.onValueChanged -= CheckPlayerOnExitTile;
    }

    private void CheckPlayerOnExitTile(Vector2Int position)
    {
        Vector2Int exitPosition = new Vector2Int(mapDefinition.Value.exitCoordinates[0], mapDefinition.Value.exitCoordinates[1]);
        if (playerPosition.Value == exitPosition && itemsLeft.Value == 0)
        {
            onPlayerFinishLevel.RaiseEvent();
            rsoLevel.Value++;
            Debug.Log("Victory");
        }
    }
}
