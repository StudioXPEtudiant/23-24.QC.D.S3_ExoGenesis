using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StudioXP.Scripts.Character
{
    [RequireComponent(typeof(AudioSource))]
    public class RandomAudioSource : MonoBehaviour
    {
        [SerializeField] private float pitchVariation = 0.2f;
        [SerializeField] private List<AudioClip> sounds;

        private AudioSource _audioSource;
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Play()
        {
            _audioSource.clip = sounds[Random.Range(0, sounds.Count)];
            _audioSource.pitch = 1 + Random.Range(-pitchVariation, pitchVariation);
            _audioSource.Play();
        }
    }
}
