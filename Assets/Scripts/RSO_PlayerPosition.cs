using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RSO_PlayerPosition", menuName = "RSO/Player Position")]
public class RSO_PlayerPosition : ScriptableObject
{
    public Action<Vector2Int> onValueChanged;

    private Vector2Int _value;

    public Vector2Int Value
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
