using UnityEngine;
using UnityEngine.AI;
using Unity.Behavior;
using Assets.CombatSystem;
using Assets.Combat;
using Assets;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;

    private NavMeshAgent _agent;
    private BehaviorGraphAgent _graphAgent;
    private Fighter _fighter;
    private Animator _animator;
    

    void Start()
    {
        _fighter = GetComponent<NpcFighter>();
        _agent = GetComponent<NavMeshAgent>();
        _graphAgent = GetComponent<BehaviorGraphAgent>();

        _graphAgent.BlackboardReference.SetVariableValue("Speed", speed);
        _graphAgent.BlackboardReference.SetVariableValue("Health", (float)_fighter.GetStat(FighterStats.HEALTH));

        _animator = GetComponentInChildren<Animator>();
        _animator.SetFloat("MotionSpeed", 1);
    }

    private void Update()
    {
        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }

    public void Hit()
    {
        _fighter.Attack();
    }
}
