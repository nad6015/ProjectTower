using Assets.CombatSystem;
using Unity.Behavior;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject _camera;

    private void Start()
    {
        GameObject exit = GameObject.Find("DungeonExit");
        GetComponent<BehaviorGraphAgent>().SetVariableValue("exit", exit);
        GameObject.Instantiate(_camera);
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision?.gameObject.SetActive(false);
    }
}
