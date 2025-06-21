using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using CombatSystem;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack", story: "[Agent] attacks [target]", category: "Action", id: "780205cc8c1720f77c5f84a97fb47c25")]
public partial class AttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    private GameObject self;
    protected override Status OnStart()
    {
        self = Agent.Value;
        self.GetComponentInChildren<Animator>().SetBool("Attack", true);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
        self.GetComponentInChildren<Animator>().SetBool("Attack", false);
    }
}

