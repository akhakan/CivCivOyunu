using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] Transform _playerTransform;
    [SerializeField] Transform _orientationTransform;
    [SerializeField] Transform _playerVisualTransform;

    [SerializeField] private float _rotateSpeed = 7f;




    void Update()
    {
        Vector3 _viewDirection = _playerTransform.position - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        _orientationTransform.forward = _viewDirection.normalized;


        float _verticalInput = Input.GetAxisRaw("Vertical");
        float _horizontalInput = Input.GetAxisRaw("Horizontal");

       Vector3 _inputDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;

        if (_inputDirection != Vector3.zero) {
            _playerVisualTransform.forward = Vector3.Slerp(_playerVisualTransform.forward, _inputDirection.normalized, Time.deltaTime * _rotateSpeed);
               
        }


    }
}
