using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] Material ghostModeMaterial;
    [SerializeField] Material wallMaterial;
    [SerializeField] MeshRenderer wallMeshRenderer;

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
        if (value)
        {
            wallMeshRenderer.material = ghostModeMaterial;
        }
        else
        {
            wallMeshRenderer.material = wallMaterial;
        }
    }
}
