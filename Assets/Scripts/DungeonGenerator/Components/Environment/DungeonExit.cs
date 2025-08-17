using Assets.Scripts.Environment;
using System;
using UnityEngine.InputSystem;

/// <summary>
/// This class provides an event that allows any interested systems to know when the player has reached the end of a dungeon.
/// </summary>
public class DungeonExit : Interactable
{
    public event Action DungeonCleared;

    protected override void HandleInteract(InputAction.CallbackContext context)
    {
        DungeonCleared?.Invoke();
    }
}