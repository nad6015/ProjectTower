using System;
using UnityEngine;


/// <summary>
/// This class provides an event that allows any interested systems to know when the player has reached the end of a dungeon.
/// </summary>
public class DungeonExit : MonoBehaviour
{
    public event Action DungeonCleared;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "PlayerController")
        {
            DungeonCleared?.Invoke();
        }
    }
}

public abstract class Interactable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //other.GetComponent<PlayerCon>()
        }
    }
}