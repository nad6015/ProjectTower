using Assets.Combat;
using Unity.Behavior;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    private void Start()
    {
        GameObject exit = GameObject.Find("DungeonExit");
        GetComponent<BehaviorGraphAgent>().SetVariableValue("exit", exit);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    internal void AttackSelf()
    {
        
    }
}
