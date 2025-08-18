using Assets.Combat;
using Assets.PlayerCharacter;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUD : MonoBehaviour
{
    StatBar _healthBar;
    StatBar _staminaBar;
    StatBar _manaBar;
    UIDocument _document;

    void Start()
    {
        Fighter fighter = GetComponentInParent<PlayableFighter>();
        _document = GetComponent<UIDocument>();

        _healthBar = new(
            fighter,
            FighterStats.Health,
            _document.rootVisualElement.Q<VisualElement>("HealthBar").Q<ProgressBar>("ProgressBar"));
        _staminaBar = new(
            fighter,
            FighterStats.STAMINA,
            _document.rootVisualElement.Q<VisualElement>("StaminaBar").Q<ProgressBar>("ProgressBar"));
        //_manaBar = new(fighter, GetComponent<UIDocument>().rootVisualElement.Q<ProgressBar>("ManaBar"));

        //_manaBar.Hide();
    }

    private void OnEnable()
    {
        _healthBar?.OnEnable();
        _staminaBar?.OnEnable();
    }

    private void OnDisable()
    {
        _healthBar.OnDisable();
        _staminaBar.OnDisable();
    }
}
