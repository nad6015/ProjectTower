using UnityEngine;

namespace Assets.Utilities
{
    public static class GameObjectUtilities
    {
        public static T FindComponentByTag<T>(string tag)
        {
            return GameObject.FindGameObjectWithTag(tag).GetComponent<T>();
        }

        public static T NewGameObjectWithComponent<T>(string name) where T : MonoBehaviour
        {
            // Create gameobject code referenced from  - https://discussions.unity.com/t/how-do-you-create-an-empty-gameobject-in-code-and-add-it-to-the-scene/86380/4
            GameObject gameObj = new GameObject(name);
            return gameObj.AddComponent<T>();
        }
    }
}