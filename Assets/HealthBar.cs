using Assets.CombatSystem;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    Fighter _fighter;

    ProgressBar _progressBar;
    IPanel _panel;
    void Start()
    {
        _fighter.OnHealthChange += UpdateHealthBar;
        _progressBar = GetComponent<UIDocument>().rootVisualElement.Q<ProgressBar>();
        
        _progressBar.highValue = _fighter.GetStat(FighterStats.HEALTH);
        _progressBar.lowValue = 0;
    }

    private void Update()
    {
        // WorldSpace To Panel Code referenced from - https://www.whatupgames.com/blog/create-a-health-bar-that-hovers-over-the-player-with-ui-toolkit
        Vector2 pos = RuntimePanelUtils.CameraTransformWorldToPanel(_progressBar.panel, _fighter.transform.position + Vector3.up, Camera.main);
        _progressBar.transform.position = pos;
    }

    private void UpdateHealthBar()
    {
        _progressBar.value = _fighter.GetStat(FighterStats.HEALTH);
        _progressBar.title = _fighter.GetStat(FighterStats.HEALTH).ToString();
    }
}
