using FabrShooter.Core;
using System.Collections;
using UnityEngine;

namespace FabrShooter
{
    [RequireComponent(typeof(AudioSource))]
    public class LevelMusicController : MonoBehaviour
    {
        private const float TARGET_VOLUME = 0.5f;
        private const float VOLUME_CHANGE_SPEED = 0.05f;

        [SerializeField] private ComboManager.ComboState targetComboState;

        private AudioSource _audioSource;
        private ComboManager _comboManager;
        private Coroutine _coroutine;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.volume = 0;

            _comboManager = ServiceLocator.Get<ComboManager>();
            _comboManager.ComboStateChanged += ToggleMusic;
        }

        private void OnDestroy()
        {
            _comboManager.ComboStateChanged -= ToggleMusic;
        }

        private void ToggleMusic()
        {
            if (_comboManager.ComboSate >= targetComboState)
            {
                if (_coroutine != null)
                    StopCoroutine(_coroutine);

                _coroutine = StartCoroutine(StartSmoothVolumeChange(TARGET_VOLUME));
            }
            else
            {
                if (_coroutine != null)
                    StopCoroutine(_coroutine);

                _coroutine = StartCoroutine(StartSmoothVolumeChange(0));
            }
        }

        private IEnumerator StartSmoothVolumeChange(float targetVolume)
        {
            if(targetVolume > 0)
            {
                while (_audioSource.volume < targetVolume)
                {
                    yield return null; 
                    _audioSource.volume = Mathf.Lerp(_audioSource.volume, targetVolume, VOLUME_CHANGE_SPEED);
                }
            }
            else
            {
                while (_audioSource.volume > targetVolume)
                {
                    yield return null; 
                    _audioSource.volume = Mathf.Lerp(_audioSource.volume, targetVolume, VOLUME_CHANGE_SPEED);
                }
            }

            _coroutine = null;
        }
    }
}