using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RSO_MapDefinition", menuName = "RSO/Map Definition")]
public class RSO_MapDefinition : ScriptableObject
{
    public Action<MapDefinition> onValueChanged;

    private MapDefinition _value;

    public MapDefinition Value
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
