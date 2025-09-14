using Assets.Combat;
using UnityEngine;

namespace Tests.Support
{
    public class TestNpcFighter : NpcFighter
    {
        public new void TakeDamage(Fighter attacker)
        {
            base.TakeDamage(attacker);
        }
    }
}