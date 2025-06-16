using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public void ApplyDamage(CharacterStats attacker, CharacterStats target)
    {
        Debug.Log("Attack!");
    }

    public void Block()
    {
        Debug.Log("Block!");
    }

    public void CastSpell()
    {
        Debug.Log("Magic");
    }
}
