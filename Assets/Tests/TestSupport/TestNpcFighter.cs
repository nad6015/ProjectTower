using Assets.Combat;
using UnityEngine;

namespace Tests.Support
{
    public class TestNpcFighter : Enemy
    {
        public new void TakeDamage(Fighter attacker)
        {
            base.TakeDamage(attacker);
        }
    }
}