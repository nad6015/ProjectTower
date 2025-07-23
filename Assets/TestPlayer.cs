using Assets.CombatSystem;
using Unity.Behavior;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject _camera;
    private bool _isInvincible = true;
    
    private void Start()
    {
        GameObject exit = GameObject.Find("DungeonExit");
        GetComponent<BehaviorGraphAgent>().SetVariableValue("exit", exit);
        GameObject.Instantiate(_camera);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision != null && _isInvincible) 
        {
            collision.gameObject.SetActive(false);
        }
    }
}
