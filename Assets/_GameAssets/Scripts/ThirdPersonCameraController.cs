using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] Transform _playerTransform;
    [SerializeField] Transform _orientationTransform;
    [SerializeField] Transform _playerVisualTransform;

    [SerializeField] private float _rotationSpeed = 2f;
    private float _verticalInput, _horizontalInput;

    private Vector3 _viewDirection, _inputDirection;

    void Update()
    {
        _viewDirection = _playerTransform.position - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        _orientationTransform.forward = _viewDirection.normalized;


        _verticalInput = Input.GetAxisRaw("Vertical");
        _horizontalInput = Input.GetAxisRaw("Horizontal");

       _inputDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;

        if (_inputDirection != Vector3.zero) {
            _playerVisualTransform.forward = Vector3.Slerp(_playerVisualTransform.forward, _inputDirection.normalized, Time.deltaTime * _rotationSpeed);
               
        }


    }
}
