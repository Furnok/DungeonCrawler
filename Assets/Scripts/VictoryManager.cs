using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    public RSO_PlayerPosition playerPosition;
    public RSE_OnPlayerFinishLevel onPlayerFinishLevel;
    public RSO_ItemsLeft itemsLeft;
    public RSO_MapDefinition mapDefinition;

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
        Debug.Log(exitPosition);
        Debug.Log(playerPosition.Value);
        Debug.Log(itemsLeft.Value);
        if (playerPosition.Value == exitPosition && itemsLeft.Value == 0)
        {
            onPlayerFinishLevel.RaiseEvent();
            Debug.Log("Victory");
        }
    }
}
