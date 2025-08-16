using Assets.Combat;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField]
    private float _yOffset = 3;

    private Fighter _fighter;
    private StatBar _bar;

    void Start()
    {
        _fighter = GetComponentInParent<Fighter>();
        _bar = new StatBar(_fighter, FighterStats.Health, GetComponent<UIDocument>().rootVisualElement.Q<ProgressBar>());
    }

    private void Update()
    {
        _bar.PositionRelativeToCamera(Camera.main, _yOffset);
    }
}
