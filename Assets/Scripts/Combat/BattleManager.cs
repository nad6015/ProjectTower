using UnityEngine;
namespace CombatSystem
{
    public class BattleManager : MonoBehaviour
    {
        public void ApplyDamage(Fighter attacker, Fighter target)
        {
            //target -= attacker.GetStat(Fighter.FighterStats.ATTACK)
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
}