using UnityEngine;

public class FlagController : MonoBehaviour
{
    [SerializeField] private GameObject _flagPrefab;
    [SerializeField] private InputController _inputController;
    
    private GameObject _currentFlag;

    void Start() => _inputController.OnNavMeshPointSelected += SetFlagPosition;

    private void OnDestroy() => _inputController.OnNavMeshPointSelected -= SetFlagPosition;

    private void SetFlagPosition(Vector3 point)
    {
        if (_currentFlag == null)
            _currentFlag = Instantiate(_flagPrefab, point, Quaternion.identity);
        else
            _currentFlag.transform.position = point;
    }
}
