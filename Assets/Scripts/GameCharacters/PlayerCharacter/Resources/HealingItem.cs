using Assets.PlayerCharacter;
using Assets.PlayerCharacter.Resources;
using UnityEngine;

public class HealingItem : UsableItem
{
    [SerializeField]
    private int _amountToHeal;
    
    public override void Use(PlayerController player)
    {
        player.GetComponent<PlayableFighter>().Heal(_amountToHeal);
        Destroy(gameObject);
    }
}
