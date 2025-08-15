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
        
        _healthBar = new(fighter, GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("HealthBar").Q<ProgressBar>("ProgressBar"));
        //_staminaBar = new(fighter, GetComponent<UIDocument>().rootVisualElement.Q<ProgressBar>("StaminaBar"));
        //_manaBar = new(fighter, GetComponent<UIDocument>().rootVisualElement.Q<ProgressBar>("ManaBar"));

        //_manaBar.Hide();
    }
}
