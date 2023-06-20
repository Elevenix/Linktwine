using UnityEngine;

namespace Sound
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private bool playOnAwake = true;
        [SerializeField] private string soundName;

        private void Awake()
        {
            if (playOnAwake)
            {
                StartSounds();
            }
        }

        public void StartSounds()
        {
            SoundManager.PlaySound(soundName);
        }
    }
}