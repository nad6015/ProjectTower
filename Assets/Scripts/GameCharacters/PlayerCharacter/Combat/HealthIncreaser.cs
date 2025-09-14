using Assets.Combat;

namespace Assets.PlayerCharacter
{
    public class HealthIncreaser : StatIncreaser
    {
        private void Start()
        {
            Stats = FighterStats.Health;
        }
    }
}