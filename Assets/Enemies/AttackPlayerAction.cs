using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack player", story: "[Self] attacks [Player]", category: "Action", id: "4aeb169d623a4a7d023a9cb2f6c9ceeb")]
public partial class AttackPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    EnemyController enemyController;
    protected override Status OnStart()
    {
        enemyController = Self.Value.GetComponent<EnemyController>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        enemyController.Attack();
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

