using UnityEngine;
using UnityEngine.Audio;

[RequireComponent (typeof(AudioSource))]
public class CharacterAudioPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip footstepClip;
    private AudioClip attackClip;

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
}
