using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField]
    private int health = 5;

    [SerializeField]
    private int attack = 1;

    public void Attack(CharacterStats stats)
    {
        stats.health -= attack;
    }

    public void Heal()
    {
        health += 2;
    }

    public void Block()
    {
        //TODO
    }
}
