using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sound
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager _instance;
    
        private AudioSource[] _sources;
        private bool[] _sourcesPlaying;

        [SerializeField] private Sound[] sounds;
        [SerializeField] private int maxSounds = 10;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            
            DontDestroyOnLoad(gameObject);
        
            foreach (var sound in sounds)
            {
                if (sounds.Count(s => s.name == sound.name) > 1)
                {
                    Debug.LogError(
                        $"SoundManager: There are more than one sound with the same name: {sound.name}. The first one on the list will be used.");
                }
            }

            InitSoundSources();
        }

        private void InitSoundSources()
        {
            _sources = new AudioSource[maxSounds];
            _sourcesPlaying = new bool[maxSounds];
            for (var i = 0; i < maxSounds; i++)
            {
                _sources[i] = gameObject.AddComponent<AudioSource>();
            }
        }

        private void PlaySoundIntern(string soundName)
        {
            StartCoroutine(PlaySound(Array.Find(sounds, sound => sound.name == soundName)));
        }
        
        public static void PlaySound(string name)
        {
            _instance.PlaySoundIntern(name);
        }

        private IEnumerator PlaySound(Sound sound)
        {
            if (sounds == null)
            {
                Debug.LogError("SoundManager: No sounds found.");
                yield break;
            }
            var index = Array.FindIndex(_sourcesPlaying, playing => !playing);
            if (index == -1)
            {
                Debug.LogWarning(
                    $"SoundManager: No free sound sources available. Sound {sound.name} will not be played. Consider increasing the maxSounds value. actual: {maxSounds}");
                yield break;
            }

            _sourcesPlaying[index] = true;
            var source = _sources[index];
            source.clip = sound.clips[Random.Range(0, sound.clips.Length)];
            source.pitch = Random.Range(sound.pitchMin, sound.pitchMax);

            source.Play();
            yield return new WaitWhile(() => source.isPlaying);
            _sourcesPlaying[index] = false;
        }
    }
}