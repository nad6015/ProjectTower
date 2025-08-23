using Assets.DungeonGenerator;
using Assets.GameManager;
using UnityEngine;

namespace Assets.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour, IModifiableSystem
    {
        [field:SerializeField]
        public AudioClip BackgroundMusic { get; private set; }
        public AudioClip FootstepAudio { get; private set; }

        private AudioSource _audioSource;

        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayBackgroundMusic()
        {
            _audioSource.clip = BackgroundMusic;
            _audioSource.loop = true;
            _audioSource.Play();
        }

        public void StopBackgroundMusic()
        {
            _audioSource.Stop();
        }

        public void Modify(DungeonComponents modifier)
        {
            BackgroundMusic = modifier.DungeonMusic;
            FootstepAudio = modifier.FootstepAudio;
        }
    }
}