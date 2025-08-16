using UnityEngine;
using UnityEngine.AI;
using Unity.Behavior;
using Assets.Combat;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent _agent;
    private BehaviorGraphAgent _graphAgent;
    private Fighter _fighter;
    private Animator _animator;

    void Start()
    {
        _fighter = GetComponent<NpcFighter>();
        _agent = GetComponent<NavMeshAgent>();
        _graphAgent = GetComponent<BehaviorGraphAgent>();
        _animator = GetComponentInChildren<Animator>();
        _animator.SetFloat("MotionSpeed", 1);

        _graphAgent.BlackboardReference.SetVariableValue("Speed", _fighter.GetMaxStat(FighterStats.Speed));
        _graphAgent.BlackboardReference.SetVariableValue("Health", _fighter.GetMaxStat(FighterStats.Health));
        _graphAgent.BlackboardReference.SetVariableValue("Animator", _animator);
    }

    private void Update()
    {
        _animator.SetFloat("Speed", _fighter.IsAttacking() ? 1 : _agent.velocity.magnitude);
    }

    internal void Attack()
    {
        _fighter.Attack();
    }
}
