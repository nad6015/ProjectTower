using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Assets.Scripts.DungeonGenerator.Components;
using Assets;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RandomPatrol", story: "[Agent] patrols area", category: "Action/Navigation", id: "1d8f569d2b61141f3aa096165522ca0d")]
public partial class RandomPatrolAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    private Vector3 _patrolPoint;
    private NavMeshAgent _navMeshAgent;

    protected override Status OnStart()
    {
        if (Agent == null)
        {
            return Status.Failure;
        }
        DungeonRoom room = FindClosestRoom();
        if (room == null)
        {
            return Status.Failure;
        }

        _navMeshAgent = Agent.Value.GetComponent<NavMeshAgent>();
        _patrolPoint = Agent.Value.transform.position + PointUtils.RandomPointWithinBounds(room.Bounds);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (HasReachedDestination(_navMeshAgent.transform, _navMeshAgent.transform.position))
        {

        }
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }

    private DungeonRoom FindClosestRoom()
    {
        RaycastHit hit;
        
        if (Physics.SphereCast(Agent.Value.transform.position, 5, Vector3.down, out hit))
        {
            return hit.collider.GetComponentInParent<DungeonRoom>();
        }

        return null;
    }

    private bool HasReachedDestination(Transform agent, Vector3 endpoint)
    {
        return Vector3.Distance(agent.position, endpoint) < 1;
    }
}

