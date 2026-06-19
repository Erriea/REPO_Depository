using UnityEngine;

namespace CaseFileLocalSuspect.UI
{
    public class UIClickSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip clickClip;
        [SerializeField] [Range(0f, 1f)] private float volume = 0.85f;

        public void PlayClick()
        {
            if (audioSource == null || clickClip == null)
            {
                return;
            }

            audioSource.PlayOneShot(clickClip, volume);
        }
    }
}
