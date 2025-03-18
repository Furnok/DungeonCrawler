using TMPro;
using UnityEngine;

public class UI_Game : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _movementPointText;

    public RSO_MovementPoints movementPoints;

    private void OnEnable()
    {
        movementPoints.onValueChanged += UpdateMovementPointText;
    }

    private void UpdateMovementPointText(int movementPoint)
    {
        _movementPointText.text = $"Movement Points Left: {movementPoint}";
    }
}
