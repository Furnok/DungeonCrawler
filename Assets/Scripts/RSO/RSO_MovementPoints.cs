using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RSO_MovementPoints", menuName = "RSO/RSO_MovementPoints")]
public class RSO_MovementPoints : ScriptableObject
{
    public Action<int> onValueChanged;

    private int _value;

    public int Value
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
