using System;
using UnityEngine;

namespace Assets.Scripts.Character
{
    [RequireComponent(typeof(AudioSource))]
    public class AnimationEventsHandler : MonoBehaviour
    {

        [SerializeField]
        private AudioClip footstepClip;
        private AudioClip attackClip;

        public event Action OnAnimationEndHandler;
        //public event Action OnAnimationEnd;
        // TODO: Move audio to different class
        private AudioSource audioSource;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void Hit()
        {
            // TODO: Play audio
        }

        public void OnFootstep()
        {
            audioSource.clip = footstepClip;
            audioSource.Play();
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