using Assets.Combat;
using Assets.PlayerCharacter;
using Assets.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField]
    private UIDocument _document;

    [SerializeField]
    PlayableFighter _fighter;

    [SerializeField]
    private GameObject _prompt;

    private StatBar _healthBar;
    private StatBar _staminaBar;

    void Start()
    {
        _document = GetComponent<UIDocument>();

        _healthBar = new StatBar(
            _fighter,
            FighterStats.Health,
            _document.rootVisualElement.Q<VisualElement>("HealthBar").Q<ProgressBar>("ProgressBar"));

        _staminaBar = new(
            _fighter,
            FighterStats.STAMINA,
            _document.rootVisualElement.Q<VisualElement>("StaminaBar").Q<ProgressBar>("ProgressBar"));
        _prompt.SetActive(false);
    }

    /// <summary>
    /// Shows the interaction prompt.
    /// </summary>
    public void ShowPrompt()
    {
        _prompt.SetActive(true);
    }

    /// <summary>
    /// Hides the interaction prompt.
    /// </summary>
    public void HidePrompt()
    {
        _prompt.SetActive(false);
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
