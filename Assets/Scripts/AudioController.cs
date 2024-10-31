using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer; // ������ �� ������
    [SerializeField] private Toggle _musicToggle; // Toggle ��� ������� ������
    [SerializeField] private Toggle _sfxToggle; // Toggle ��� ������ ����

    private void Start()
    {
        // ����������� ������ �� ������� �������
        _musicToggle.onValueChanged.AddListener(ToggleMusic);
        _sfxToggle.onValueChanged.AddListener(ToggleSFX);

        // �������������� ��������� (��������, ���������� ��� �������)
        _audioMixer.SetFloat("MusicVolume", 0); // �������� ������
        _audioMixer.SetFloat("SFXVolume", 0);   // �������� �����
    }

    private void ToggleMusic(bool isOn)
    {
        _audioMixer.SetFloat("MusicVolume", isOn ? 0 : -80); // -80 �� ��� ������� ����������
    }

    private void ToggleSFX(bool isOn)
    {
        _audioMixer.SetFloat("SFXVolume", isOn ? 0 : -80); // -80 �� ��� ������� ����������
    }

    private void OnDestroy()
    {
        // ���������� ������ �� �������, ����� �������� ������ ������
        _musicToggle.onValueChanged.RemoveListener(ToggleMusic);
        _sfxToggle.onValueChanged.RemoveListener(ToggleSFX);
    }
}
