# Unity Kamera Tabanlı Hareket -- Sayısal Analizli Açıklama

## 1. Kodun Satır Satır Açıklaması

### `[SerializeField] Transform _playerTransform;`

Inspector'dan atanabilen bir `Transform`.\
→ Oyuncunun pozisyon ve rotasyon bilgisi.

### `[SerializeField] Transform _orientationTransform;`

**Kameraya göre ileri yönü** belirleyen boş obje.\
→ Karakterin hareket edeceği yön: `orientation.forward`.

### `[SerializeField] Transform _playerVisualTransform;`

Oyuncunun sadece görsel modelidir.
→ Hareket yönüne doğru döndürülür.

### `private float _rotationSpeed = 6f;`

Slerp **dönüş** hızını kontrol eder.

### `private float _verticalInput, _horizontalInput;`

Input değerlerini saklar.

### `Update()`

Kullacının W, A, S, D veya joystick girdilerini okur.

-   W → Vertical = +1
-   S → Vertical = -1
-   D → Horizontal = +1
-   A → Horizontal = -1

`GetAxisRaw()` → (-1 / 0 / +1) hızlı tepki verir.

------------------------------------------------------------------------

# 🎯 SAYISAL ÖRNEKLERLE KOD ÇÖZÜMÜ

### Varsayımlar:

**Player pozisyonu:**\
(5, 0, 5)

**Kamera pozisyonu:**\
(1, 3, 1)

**Input (W + D):**\
Vertical = 1\
Horizontal = 1

------------------------------------------------------------------------

## 🔵 1) `viewDirection` Hesaplama

Kod:
```csharp
    _viewDirection =
        _playerTransform.position -
        new Vector3(transform.position.x, _playerTransform.position.y, transform.position.z);
```
Hesap:
```ini
camera.y → player.y yapılır:

    camera: (1, 3, 1)
    y = 0 yapılır → (1, 0, 1)

    _viewDirection = (5,0,5) - (1,0,1)
                   = (4,0,4)
```
Normalize:

    length = sqrt(4² + 4²) = 5.657
    normalized = (0.707, 0, 0.707)

------------------------------------------------------------------------

## 🔵 2) `orientationTransform.forward`

    orientation.forward = (0.707, 0, 0.707)

------------------------------------------------------------------------

## 🔵 3) Input'a Göre Hareket Yönü

Kod:

    _inputDirection =
        orientation.forward * verticalInput +
        orientation.right   * horizontalInput;

forward = (0.707, 0, 0.707)\
right = (0.707, 0, --0.707)

Hesap:

    forward * 1  = (0.707, 0, 0.707)
    right   * 1  = (0.707, 0, -0.707)
    ---------------------------------
    toplam       = (1.414, 0, 0)

Normalize → (1, 0, 0)\
➡ W + D → karakter kameraya göre sağa gider.

------------------------------------------------------------------------

## 🔵 4) Player Görselinin Döndürülmesi

Kod:

    _playerVisualTransform.forward = Vector3.Slerp(
        _playerVisualTransform.forward,
        _inputDirection.normalized,
        Time.deltaTime * _rotationSpeed
    );

rotation hız örneği:

    rotationSpeed = 6
    deltaTime = 0.016
    Slerp factor = 0.096

Model her frame'de hedefe %9.6 yaklaşır.

------------------------------------------------------------------------

# 🟩 ÖZET

  Adım                   Sonuç
  ---------------------- ---------------------
  Kamera → Player yönü   (0.707, 0, 0.707)
  Orientation forward    (0.707, 0, 0.707)
  Orientation right      (0.707, 0, --0.707)
  Input (W+D) sonucu     (1, 0, 0)
  Player görseli         Sağa döner
