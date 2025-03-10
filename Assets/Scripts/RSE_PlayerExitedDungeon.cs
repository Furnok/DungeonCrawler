using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RSE_PlayerExitedDungeon", menuName = "RSE/Player Exited Dungeon")]
public class RSE_PlayerExitedDungeon : ScriptableObject
{
    public Action Dispatch;
}