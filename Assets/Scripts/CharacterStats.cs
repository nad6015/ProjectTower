using UnityEngine;

public struct CharacterStats
{
    public int health;
    public int attack;

    public CharacterStats(int health, int attack)
    {
        this.health = health;
        this.attack = attack;
    }
}
