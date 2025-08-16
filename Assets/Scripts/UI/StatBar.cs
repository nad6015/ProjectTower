using Assets.Combat;
using UnityEngine;
using UnityEngine.UIElements;

public class StatBar
{
    private readonly Fighter _fighter;
    private readonly ProgressBar _bar;
    private readonly FighterStats _stat;

    public StatBar(Fighter fighter, FighterStats stat, ProgressBar progressBar)
    {
        _fighter = fighter;
        _bar = progressBar;

        _fighter.OnStatChange += UpdateStatBar;

        _bar.highValue = _fighter.GetMaxStat(stat);
        _bar.lowValue = 0;
        _stat = stat;
    }

    internal void PositionRelativeToCamera(Camera main, float yOffset = 3)
    {
        // WorldSpace To Panel Code referenced from - https://www.whatupgames.com/blog/create-a-health-bar-that-hovers-over-the-player-with-ui-toolkit
        Vector2 pos = RuntimePanelUtils.CameraTransformWorldToPanel(_bar.panel,
           _fighter.transform.position + (yOffset * Vector3.up), main);
        _bar.transform.position = new(pos.x - (_bar.contentRect.width / 2f), pos.y);
    }

    private void UpdateStatBar(FighterStats stat)
    {
        if (_stat == stat)
        {
            _bar.value = _fighter.GetStat(stat);
        }
    }

    internal void Hide()
    {
        _bar.AddToClassList("hidden");
    }
}
