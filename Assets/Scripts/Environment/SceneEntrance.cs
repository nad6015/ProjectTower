using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Assets.Interactables.Environment
{
    /// <summary>
    /// Transitions the player to a new scene.
    /// </summary>
    public class SceneEntrance : Interactable
    {
        [SerializeField]
        private string _sceneName;

        public event Action DungeonCleared;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="context">the input context</param>
        protected override void HandleInteract(InputAction.CallbackContext context)
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}