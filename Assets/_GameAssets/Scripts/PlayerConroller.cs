using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] private Transform _movementOrientation; // Oryantasyon game objectini koda baðlama -> Sürükle Býrak
    private Rigidbody _playerRigidbody; // player Rigidbody' eriþmek için
    private float _verticalInput, _horizontalInput; // ileri-geri klavye giriþleri tutan deðiþken, saða-sola klavye giriþleri tutan deðiþken
    private Vector3 _movementDirection; // klavye giriþlerine göre player ýn yönünü vektör 3 formatýnda tutan deðiþken
    [SerializeField] private float _movementSpeed = 20f; // player a uygulanacak kuvvetin þiddetini arttýran çarpan
    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>(); // player gameobjesinin rigidbody sine eriþme 
        _playerRigidbody.freezeRotation = true; // player x,y,z düzlemlerinde rotasyona uðramasýný engellemek için kullanýlýr.
    }

    private void Update()
    {
        _verticalInput = Input.GetAxisRaw("Vertical");//1 -1  // klavyeden hangi tuþa basýldý ileri ok - geri ok ya da W-S tuþlarý  GetAxis 0-1 0.2-0.5-0.7-0.9 
        _horizontalInput = Input.GetAxisRaw("Horizontal");//1 -1 // klavyeden hangi tuþa basýldý sað ok - sol ok ya da A-D tuþlarý
        _movementDirection= _movementOrientation.forward *_verticalInput + _movementOrientation.right*_horizontalInput;        //(-1,0,0)        //(X,Y,Z)    
        //Vektör 3 yönünün tüm koordinatlarda hesaplanmasý
    }


    private void FixedUpdate()
    {
        _playerRigidbody.AddForce(_movementDirection.normalized * _movementSpeed, ForceMode.Force); // player hareti saðlanýr. Rigidbody fizik gerektirdiðinden FixedUpdate içine yazýlmalýdýr. Normalized: ileri-saða veya geri-saða veya ileri-sola veya geri-sola tuþlarýna birlikte basýlýrsa vektör toplamýný 1 deðerine yuvarlar. ForceMode.Force: Sürekli sabit güç uygular.
    }



}
