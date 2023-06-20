using System;
using UnityEngine;

namespace Sound
{
    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip[] clips;
        public float pitchMin = 1;
        public float pitchMax = 1;
    }
}