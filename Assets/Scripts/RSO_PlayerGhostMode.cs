using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RSO_PlayerGhostMode", menuName = "RSO/Player ghost mode")]
public class RSO_PlayerGhostMode : ScriptableObject
{
    public Action<bool> onValueChanged;

    private bool _value;

    public bool Value
    {
        get => _value;
        set
        {
            if (_value == value) return;

            _value = value;
            onValueChanged?.Invoke(_value);
        }
    }
}
