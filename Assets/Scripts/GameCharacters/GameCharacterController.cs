using Assets.Combat;
using UnityEngine;

namespace Assets.GameCharacters
{
    public abstract class GameCharacterController : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _attackAudio;

        [SerializeField]
        protected AudioClip _footstepAudio;

        private AudioSource _audioSource;
        private AnimationEventsHandler _animationEventsHandler;
        //protected AudioManager _audioManager;

        protected virtual void Start()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
            _animationEventsHandler = GetComponentInChildren<AnimationEventsHandler>();

            _animationEventsHandler.OnFootstepHandler += Footstep;
            _animationEventsHandler.OnHitHandler += Hit;
            _animationEventsHandler.OnAnimationEndHandler += GetComponent<Fighter>().OnAnimationEnd;

            //_audioManager = FindComponentByTag<AudioManager>()
        }

        public void Hit()
        {
            _audioSource.clip = _attackAudio;
            _audioSource.volume = 0.5f;
            _audioSource.Play();
        }

        public void Footstep()
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.clip = _footstepAudio;
                _audioSource.volume = 0.5f;
                _audioSource.Play();
            }
        }

        protected void PlaySound(AudioClip clip)
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.clip = clip;
                _audioSource.Play();
            }
        }
    }
}