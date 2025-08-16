using Assets.Combat;
using Assets.PlayerCharacter;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUD : MonoBehaviour
{
    StatBar _healthBar;
    StatBar _staminaBar;
    StatBar _manaBar;

    void Start()
    {
        Fighter fighter = GetComponentInParent<PlayableFighter>();
        
        _healthBar = new(
            fighter, 
            FighterStats.Health, 
            GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("HealthBar").Q<ProgressBar>("ProgressBar"));
        _staminaBar = new(
            fighter, 
            FighterStats.STAMINA, 
            GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("StaminaBar").Q<ProgressBar>("ProgressBar"));
        //_manaBar = new(fighter, GetComponent<UIDocument>().rootVisualElement.Q<ProgressBar>("ManaBar"));

        //_manaBar.Hide();
    }
}
