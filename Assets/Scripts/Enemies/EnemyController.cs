using UnityEngine;
using UnityEngine.AI;
using Unity.Behavior;
using Assets.Combat;
using Assets.Combat;
using Assets;
using System;

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
        _animator = GetComponentInChildren<Animator>();
        _animator.SetFloat("MotionSpeed", 1);

        _graphAgent.BlackboardReference.SetVariableValue("Speed", speed);
        _graphAgent.BlackboardReference.SetVariableValue("Health", (float)_fighter.GetStat(FighterStats.HEALTH));
        _graphAgent.BlackboardReference.SetVariableValue("Animator", _animator);
    }

    private void Update()
    {
        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }

    public void Hit()
    {
      
    }

    internal void Attack()
    {
        _fighter.Attack();
    }
}
