using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private RSE_OnButtonPlayPress _rseOnButtonPlayPress;

    private void OnEnable()
    {
        _rseOnButtonPlayPress.action += OnButtonPlayPress;
    }

    private void OnDisable()
    {
        _rseOnButtonPlayPress.action -= OnButtonPlayPress;
    }


    private void OnButtonPlayPress()
    {
        SceneManager.LoadScene("Scene_Game");
        Debug.Log("Button Play Pressed");
    }
}
