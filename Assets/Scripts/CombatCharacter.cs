using UnityEngine;

public class CombatCharacter : MonoBehaviour
{
    [SerializeField]
    private int health = 5;

    [SerializeField]
    private int attack = 1;

    public CharacterStats stats;
    void Start()
    {
        stats = new(health, attack);
    }

    public void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.gameObject.GetComponent<EnemyCharacter>();
        if (enemy != null) {
            Debug.Log("Character!");
            enemy.TakeDamage(attack);
        }
    }

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
