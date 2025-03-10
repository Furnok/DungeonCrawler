using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public RSO_PlayerPosition playerPosition;

    private void OnEnable()
    {
        playerPosition.onValueChanged += Move;
    }

    private void OnDisable()
    {
        playerPosition.onValueChanged -= Move;
    }

    private void Move(Vector2Int movement)
    {
        Vector3 destination = transform.position;
        destination.x = movement.x;
        destination.z = movement.y;

        transform.position = destination;
    }
}
