using Assets.PlayerCharacter;
using Unity.Behavior;
using UnityEngine;

public class TestPlayer : PlayerController
{
    protected override void Start()
    {
        base.Start();
        //GameObject exit = GameObject.Find("DungeonExit");
        //GetComponent<BehaviorGraphAgent>().SetVariableValue("exit", exit);
    }

    internal void AttackSelf()
    {
        
    }
}
