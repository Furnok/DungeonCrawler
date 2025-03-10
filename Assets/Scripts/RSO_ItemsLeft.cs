using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RSO_ItemsLeft", menuName = "RSO/Items Left")]
public class RSO_ItemsLeft : ScriptableObject
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
