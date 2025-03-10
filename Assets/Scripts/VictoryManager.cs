using System;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    public RSO_PlayerPosition playerPosition;
    public RSO_ExitPosition exitPosition;
    public RSE_PlayerExitedDungeon playerExitedDungeon;

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
        if (playerPosition.Value == exitPosition.Value)
            playerExitedDungeon.Dispatch();
    }
}
