using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Toggle _musicToggle;
    [SerializeField] private Toggle _sfxToggle;

    private string _musicVolume = "MusicVolume";
    private string _sfxVolume = "SFXVolume";

    private void Start()
    {
        _musicToggle.onValueChanged.AddListener(ToggleMusic);
        _sfxToggle.onValueChanged.AddListener(ToggleSFX);
        _audioMixer.SetFloat(_musicVolume, 0);
        _audioMixer.SetFloat(_sfxVolume, 0);
    }

    private void ToggleMusic(bool isOn) => _audioMixer.SetFloat(_musicVolume, isOn ? 0 : -80);

    private void ToggleSFX(bool isOn) =>_audioMixer.SetFloat(_sfxVolume, isOn ? 0 : -80);

    private void OnDestroy()
    {
        _musicToggle.onValueChanged.RemoveListener(ToggleMusic);
        _sfxToggle.onValueChanged.RemoveListener(ToggleSFX);
    }
}
