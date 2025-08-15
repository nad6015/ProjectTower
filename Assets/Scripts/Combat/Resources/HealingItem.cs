using Assets.Combat;
using Assets.Scripts.Combat.Resources;
using UnityEngine;

public class HealingItem : Item
{
    [SerializeField]
    private int _amountToHeal;
    
    public override void Use(Fighter player)
    {
        player.Heal(_amountToHeal);
    }
}
