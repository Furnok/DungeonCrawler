using UnityEngine;

public class RSE_OnReachEnd : MonoBehaviour
{
    private AudioSource _audioSource;
    public RSO_ItemsLeft _itemsLeft;
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _itemsLeft.onValueChanged += PlaySound;
    }

    private void OnDisable()
    {
        _itemsLeft.onValueChanged -= PlaySound;
    }

    void PlaySound(int value)
    {
        _audioSource.Play();
    }
}
