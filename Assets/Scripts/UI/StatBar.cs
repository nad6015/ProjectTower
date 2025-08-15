using Assets.Combat;
using UnityEngine;
using UnityEngine.UIElements;

public class StatBar
{
    private Fighter _fighter;
    private ProgressBar _bar;

    public StatBar(Fighter fighter, ProgressBar progressBar)
    {
        _fighter = fighter;
        _bar = progressBar;

        _fighter.OnHealthChange += UpdateHealthBar;

        _bar.highValue = _fighter.GetStat(FighterStats.HEALTH);
        _bar.lowValue = 0;
    }  

    internal void PositionRelativeToCamera(Camera main, float yOffset = 3)
    {
        // WorldSpace To Panel Code referenced from - https://www.whatupgames.com/blog/create-a-health-bar-that-hovers-over-the-player-with-ui-toolkit
        Vector2 pos = RuntimePanelUtils.CameraTransformWorldToPanel(_bar.panel,
           _fighter.transform.position + (yOffset * Vector3.up), main);
        _bar.transform.position = new(pos.x - (_bar.contentRect.width / 2f), pos.y);
    }

    private void UpdateHealthBar()
    {
        _bar.value = _fighter.GetStat(FighterStats.HEALTH);
    }

    internal void Hide()
    {
        _bar.AddToClassList("hidden");
    }
}
