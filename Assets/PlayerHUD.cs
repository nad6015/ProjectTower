using Assets.CombatSystem;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField]
    Fighter _fighter;

    ProgressBar _healthBar;

    void Start()
    {
        _fighter.OnDamageTaken += UpdateHealthBar;
        _healthBar = GetComponent<UIDocument>().rootVisualElement.Q<ProgressBar>("HealthBar");;

        _healthBar.highValue = _fighter.GetStat(FighterStats.HEALTH);
        _healthBar.lowValue = 0;
    }

    private void UpdateHealthBar()
    {
        _healthBar.value = _fighter.GetStat(FighterStats.HEALTH);
        _healthBar.title = _fighter.GetStat(FighterStats.HEALTH).ToString();
    }
}
