using Assets.CombatSystem;
using Unity.Behavior;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    internal bool isInvincible = true;
    private void Start()
    {
        GameObject exit = GameObject.Find("DungeonExit");
        GetComponent<BehaviorGraphAgent>().SetVariableValue("exit", exit);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision != null && isInvincible) 
        {
            collision.gameObject.SetActive(false);
        }
    }
}
