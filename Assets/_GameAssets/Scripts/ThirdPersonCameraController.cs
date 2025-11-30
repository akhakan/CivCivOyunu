using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] Transform _playerTransform; // Oyuncunun Transform bileþeni (konum/rotasyon) - kamerayla olan konumsal iliþki için
    [SerializeField] Transform _orientationTransform; // Yönelim (orientation) için kullanýlan boþ GameObject'in Transform'u - input yönünü belirlemek için
    [SerializeField] Transform _playerVisualTransform; // Oyuncu modelinin/visual'ýnýn Transform'u - oyuncu görselini döndürmek için

    [SerializeField] private float _rotationSpeed = 6f; // Oyuncu görselinin dönme hýzýný kontrol eder
    private float _verticalInput, _horizontalInput; // Klavye/joystick input deðerlerini saklamak için

    private Vector3 _viewDirection, _inputDirection; // Kameraya bakýþ yönü ve oyuncunun hareket input yönü

    void Update()
    {
        // Kullanýcý inputlarýný oku. GetAxisRaw anlýk ham input verir (sürekli deðil, -1/0/1 yakýn deðerler)
        _verticalInput = Input.GetAxisRaw("Vertical");
        _horizontalInput = Input.GetAxisRaw("Horizontal");
            }
    private void LateUpdate()
    {
        // Kamera ile oyuncu arasýndaki yatay (Y ekseni sabit) bakýþ vektörünü hesapla.
        // transform.position.x/z kullanýlarak kamera pozisyonunun X ve Z'si alýnýr,
        // ancak Y deðeri oyuncunun Y'si ile eþitlenir -> böylece yukarý-aþaðý farký görmezden gelerek sadece yatay yön hesaplanýr.
        _viewDirection = _playerTransform.position - new Vector3(transform.position.x, _playerTransform.position.y, transform.position.z);
        // Hesaplanan bakýþ vektörünü orientation transformunun forward (ileri) yönü yap.
        // orientation, oyuncunun 'ilerisi' olarak kabul edilecektir; input yönünü buna göre dönüþtüreceðiz.
        _orientationTransform.forward = _viewDirection.normalized;


        // Orientation'ýn forward ve right vektörlerini kullanarak dünya uzayýnda hareket yönünü hesapla.
        // vertical input -> ileri/geri, horizontal input -> sað/sol.
        // Böylece kameraya göre yönlendirilmiþ bir input vektörü elde edilir.
        _inputDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;

        // Eðer input yönü sýfýr deðilse (yani oyuncu bir yönde hareket etmeye çalýþýyorsa) oyuncu modelini döndür.
        if (_inputDirection != Vector3.zero)
        {
            // Slerp ile yumuþak bir dönüþ uygula: mevcut forward yönünden hedef input yönüne doðru geçiþ yap.
            // Time.deltaTime ile frame baðýmsýz hýz, _rotationSpeed ile dönüþ hýzý kontrol edilir.
            _playerVisualTransform.forward = Vector3.Slerp(_playerVisualTransform.forward, _inputDirection.normalized, Time.deltaTime * _rotationSpeed);

        }
    }
}

