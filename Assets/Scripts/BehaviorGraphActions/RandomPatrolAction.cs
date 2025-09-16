using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Assets;
using UnityEngine.AI;
using Assets.DungeonGenerator.Components;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RandomPatrol", story: "[Agent] randomly patrols its current room", category: "Action", id: "1d8f569d2b61141f3aa096165522ca0d")]
public partial class RandomPatrolAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<float> Speed = new(1.0f);
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
        
        GameObject gameObject = Agent.Value;

        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

        _patrolPoint = PointUtils.RandomPointWithinBounds(room.Bounds);
        _navMeshAgent.destination = _patrolPoint;
       
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (HasReachedDestination(_navMeshAgent.transform, _patrolPoint))
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        
    }

    private DungeonRoom FindClosestRoom()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(Agent.Value.transform.position, Vector3.down, out hit))
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

