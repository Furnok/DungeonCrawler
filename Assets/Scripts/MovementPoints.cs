using System.Collections.Generic;
using UnityEngine;

public class MovementPoints : MonoBehaviour
{
    [SerializeField] private List<int> movementPointsMax = new List<int>();

    public RSO_LevelNumber _levelNumber;
    public RSE_OnPlayerFinishLevel onPlayerFinishLevel;
    public RSE_OnPlayerMove onPlayerMove;
    public RSE_OnPlayerDie onPlayerDie;
    public RSO_MovementPoints movementPoints;

    private int currentMovementPoints;

    private void Start()
    {
        if (movementPointsMax.Count == 0)
        {
            Debug.LogError("No movement points max defined");
            return;
        }

        movementPoints.Value = 0;
        currentMovementPoints = movementPointsMax[0];
        movementPoints.Value = currentMovementPoints;
    }

    private void OnEnable()
    {
        onPlayerMove.action += DecreaseMovementPoints;
        _levelNumber.onValueChanged += SetMovementPoints;
    }

    private void OnDisable()
    {
        onPlayerMove.action -= DecreaseMovementPoints;
        _levelNumber.onValueChanged -= SetMovementPoints;
    }

    void DecreaseMovementPoints()
    {
        currentMovementPoints--;
        movementPoints.Value = currentMovementPoints;

        if (currentMovementPoints <= 0)
        {
            onPlayerDie.RaiseEvent();
        }
    }

    void SetMovementPoints(int level)
    {
        if(level >= movementPointsMax.Count)
        {
            Debug.LogError("Level number is higher than the number of movement points max defined");
            currentMovementPoints = 0;
            return;
        }

        currentMovementPoints = movementPointsMax[level];
        movementPoints.Value = currentMovementPoints;

    }
}
