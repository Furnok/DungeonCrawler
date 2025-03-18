using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveDuration = 0.5f;

    public RSO_PlayerPosition playerPosition;
    public RSO_PlayerIsMoving playerIsMoving;

    private void Awake()
    {
        playerIsMoving.Value = false;
    }
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
        Vector3 destination = new Vector3(movement.x, transform.position.y, movement.y);
        StartCoroutine(SmoothMove(destination));
    }

    private IEnumerator SmoothMove(Vector3 destination)
    {
        playerIsMoving.Value = true;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, destination, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = destination;
        playerIsMoving.Value = false;
    }
}
