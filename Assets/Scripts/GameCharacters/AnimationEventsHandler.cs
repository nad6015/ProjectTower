using System;
using UnityEngine;

namespace Assets.GameCharacters
{
    [RequireComponent(typeof(AudioSource))]
    public class AnimationEventsHandler : MonoBehaviour
    {
        public event Action OnAnimationEndHandler;
        public event Action OnHitHandler;
        public event Action OnFootstepHandler;

        public void Hit()
        {
            OnHitHandler?.Invoke();
        }

        public void OnFootstep()
        {
            OnFootstepHandler?.Invoke();
        }

        /// <summary>
        /// Handler for any animation with the OnAnimationEnd event.
        /// </summary>
        public void OnAnimationEnd()
        {
            OnAnimationEndHandler?.Invoke();
        }
    }
}