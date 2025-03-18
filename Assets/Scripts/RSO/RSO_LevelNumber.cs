using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RSO_LevelNumber", menuName = "RSO/RSO_LevelNumber")]
public class RSO_LevelNumber : ScriptableObject
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
