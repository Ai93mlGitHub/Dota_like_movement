using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer; // Ссылка на микшер
    [SerializeField] private Toggle _musicToggle; // Toggle для фоновой музыки
    [SerializeField] private Toggle _sfxToggle; // Toggle для звуков игры

    private void Start()
    {
        // Подписываем методы на события нажатия
        _musicToggle.onValueChanged.AddListener(ToggleMusic);
        _sfxToggle.onValueChanged.AddListener(ToggleSFX);

        // Инициализируем громкость (например, активируем при запуске)
        _audioMixer.SetFloat("MusicVolume", 0); // Включаем музыку
        _audioMixer.SetFloat("SFXVolume", 0);   // Включаем звуки
    }

    private void ToggleMusic(bool isOn)
    {
        _audioMixer.SetFloat("MusicVolume", isOn ? 0 : -80); // -80 дБ для полного отключения
    }

    private void ToggleSFX(bool isOn)
    {
        _audioMixer.SetFloat("SFXVolume", isOn ? 0 : -80); // -80 дБ для полного отключения
    }

    private void OnDestroy()
    {
        // Отписываем методы от событий, чтобы избежать утечек памяти
        _musicToggle.onValueChanged.RemoveListener(ToggleMusic);
        _sfxToggle.onValueChanged.RemoveListener(ToggleSFX);
    }
}
