using UnityEngine;

namespace Assets.Utilities
{
    public static class GameObjectUtilities
    {
        public static T GetComponentByGameObjectTag<T>(string tag)
        {
            return GameObject.FindGameObjectWithTag(tag).GetComponent<T>();
        }
    }
}