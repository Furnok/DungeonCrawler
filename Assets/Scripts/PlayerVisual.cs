using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] MeshRenderer playerMeshRenderer;
    [SerializeField] Material playerMaterial;
    [SerializeField] Material playerGhostModeMaterial;

    public RSO_PlayerGhostMode playerGhostMode;

    private void OnEnable()
    {
        playerGhostMode.onValueChanged += HandlePlayerGhostModeChange;
    }

    private void OnDisable()
    {
        playerGhostMode.onValueChanged -= HandlePlayerGhostModeChange;
    }

    void HandlePlayerGhostModeChange(bool value)
    {
        if(value)
        {
            playerMeshRenderer.material = playerGhostModeMaterial;
        }
        else
        {
            playerMeshRenderer.material = playerMaterial;
        }
    }
}
