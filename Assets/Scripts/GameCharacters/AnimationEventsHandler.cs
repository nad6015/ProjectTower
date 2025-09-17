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
        public event Action OnDodgeDoneHandler;
        public event Action<GameObject> OnAttackStartHandler;

        /// <summary>
        /// Event handler for any animation with the OnFootstep event.
        /// </summary>
        public void Hit()
        {
            OnHitHandler?.Invoke();
        }

        /// <summary>
        /// Event handler for any animation with the OnFootstep event.
        /// </summary>
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

        /// <summary>
        /// Handler for any animation with the DodgeDone event.
        /// </summary>
        public void DodgeDone()
        {
            OnDodgeDoneHandler?.Invoke();
        }

        /// <summary>
        /// Handler for any animation with the DodgeDone event.
        /// </summary>
        public void AttackStart(UnityEngine.Object gameObject)
        {
            OnAttackStartHandler?.Invoke((GameObject)gameObject);
        }
    }
}