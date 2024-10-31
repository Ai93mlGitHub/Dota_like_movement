using UnityEngine;

public class LandmineView : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosionVFX;
    [SerializeField] private AudioSource _explosionSoundPrefab; // Префаб для звука взрыва

    private void OnEnable()
    {
        Landmine landmine = GetComponent<Landmine>();
        landmine.OnExplode += Explode;
    }

    private void OnDisable()
    {
        Landmine landmine = GetComponent<Landmine>();
        landmine.OnExplode -= Explode;
    }

    private void Explode(Vector3 position, Quaternion rotation)
    {
        // Проигрываем визуальный эффект взрыва
        if (_explosionVFX != null)
            Instantiate(_explosionVFX, position, rotation);

        // Проигрываем звук взрыва
        if (_explosionSoundPrefab != null)
        {
            AudioSource explosionAudio = Instantiate(_explosionSoundPrefab, position, rotation);
            Destroy(explosionAudio.gameObject, explosionAudio.clip.length); // Удаляем объект после проигрывания звука
        }
    }
}
