using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AvoidTarget", story: "[Agent] avoids [Target]", category: "Action", id: "deb4afb1985641ec9c7697156a5a07a6")]
public partial class AvoidTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Speed;
    private GameObject self;
    private GameObject target;

    protected override Status OnStart()
    {
        self = Agent.Value;
        target = Target.Value;
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Vector3.Distance(self.transform.position, target.transform.position) < 1f) { 
            var dirAwayFromTarget = target.transform.position - self.transform.position;
            Debug.Log(dirAwayFromTarget);
            Debug.Log(dirAwayFromTarget.normalized);
            self.GetComponent<NavMeshAgent>().SetDestination(dirAwayFromTarget.normalized * Speed.Value);
            return Status.Running;
        }
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

