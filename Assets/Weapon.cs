using CombatSystem;
using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private float weaponReach =3f;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAction>().PlayerAttacks += onPlayerAttacks;
    }

    private void OnDisable()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAction>().PlayerAttacks -= onPlayerAttacks;
    }

    private void onPlayerAttacks(object sender, System.EventArgs e)
    {
        
    }

    internal float WeaponReach()
    {
        return weaponReach;
    }
}
