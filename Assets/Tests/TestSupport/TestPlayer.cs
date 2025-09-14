using Assets.PlayerCharacter;
using Unity.Behavior;
using UnityEngine;

namespace Tests.Support
{
    public class TestPlayer : PlayerController
    {
        TestPlayableFighter _fighter;
        protected override void Start()
        {
            base.Start();
            _fighter = GetComponent<TestPlayableFighter>();
            
        }
    }
}