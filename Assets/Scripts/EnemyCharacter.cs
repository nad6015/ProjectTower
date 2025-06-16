using UnityEngine;

public class EnemyCharacter : MonoBehaviour, ICharacter
{
    [SerializeField]
    private int health = 5;

    [SerializeField]
    private int attack = 1;

    private CharacterStats stats;

    void Start()
    {
        stats = new (health, attack);
    }
    public void Attack(CharacterStats stats)
    {
        throw new System.NotImplementedException();
    }

    public void Block()
    {
        throw new System.NotImplementedException();
    }

    public void Heal()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(float damage)
    {
        
    }
}
