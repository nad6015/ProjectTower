using System;
using UnityEngine;

namespace Assets.GameManager
{
    public class TransitionAnimator : MonoBehaviour
    {
        public event Action TransitionComplete;

        [SerializeField]
        private AnimationClip _transitionClip;

        private Animator _animator;
       

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void Play()
        {
            _animator.SetTrigger("CrossFade");
        }

        public void OnTransitionEnd() 
        {
            TransitionComplete?.Invoke();
        }
    }
}