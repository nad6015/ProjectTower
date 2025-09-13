using UnityEngine;

public static class TestUtilities
{
    public static bool HasReachedDestination(Transform agent, Transform endpoint)
    {
        return Vector3.Distance(endpoint.position, agent.position) <= 1.5f;
    }
}