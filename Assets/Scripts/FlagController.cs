using UnityEngine;

public class FlagController : MonoBehaviour
{
    [SerializeField] private GameObject _flagPrefab;
    
    private GameObject _currentFlag;

    void Start() => InputController.OnClick += SetFlagPosition;

    private void OnDestroy() =>InputController.OnClick -= SetFlagPosition;

    private void SetFlagPosition(Vector3 point)
    {
        if (_currentFlag == null)
            _currentFlag = Instantiate(_flagPrefab, point, Quaternion.identity);
        else
            _currentFlag.transform.position = point;
    }
}
