using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RSO_PlayerIsMoving", menuName = "RSO/RSO_PlayerIsMoving")]
public class RSO_PlayerIsMoving : ScriptableObject
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
