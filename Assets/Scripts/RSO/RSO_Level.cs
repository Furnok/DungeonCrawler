using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RSO_Level", menuName = "RSO/RSO_Level")]
public class RSO_Level : ScriptableObject
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
