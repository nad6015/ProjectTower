using UnityEngine;
using UnityEngine.AI;
using Unity.Behavior;
using Assets.Combat;
using Assets.GameCharacters;

public class EnemyController : GameCharacterController
{
    [SerializeField]
    private float _vocalizationCooldownMax = 15f;

    [SerializeField]
    private AudioClip _vocalizationAudio;

    private NavMeshAgent _agent;
    private BehaviorGraphAgent _graphAgent;
    private Fighter _fighter;
    private Animator _animator;
    private float _vocalizationCooldown;

    protected override void Start()
    {
        base.Start();
        _fighter = GetComponent<NpcFighter>();
        _agent = GetComponent<NavMeshAgent>();
        _graphAgent = GetComponent<BehaviorGraphAgent>();
        _animator = GetComponentInChildren<Animator>();
        _animator.SetFloat("MotionSpeed", 1);

        _graphAgent.BlackboardReference.SetVariableValue("Speed", _fighter.GetMaxStat(FighterStats.Speed));
        _graphAgent.BlackboardReference.SetVariableValue("Health", _fighter.GetMaxStat(FighterStats.Health));
        _graphAgent.BlackboardReference.SetVariableValue("Animator", _animator);

        _fighter.OnStatChange += HandleStatChange;

        _vocalizationCooldown = Random.Range(0, _vocalizationCooldownMax);
    }


    private void Update()
    {
        _animator.SetFloat("Speed", _fighter.IsAttacking() ? _fighter.GetStat(FighterStats.Speed) : _agent.velocity.magnitude);
        _vocalizationCooldown -= Time.deltaTime;
        if (_vocalizationCooldown <= 0)
        {
            PlaySound(_vocalizationAudio);
            _vocalizationCooldown = Random.Range(0, _vocalizationCooldownMax);
        }
    }

    /// <summary>
    /// A delegating method called when the behaviour tree decides to attak the player.
    /// </summary>
    public void Attack()
    {
        _fighter.Attack();
    }

    private void HandleStatChange(FighterStats stat)
    {
        if (stat == FighterStats.Health)
        {
            _graphAgent.BlackboardReference.SetVariableValue("Health", _fighter.GetStat(FighterStats.Health));
        }
    }
}
