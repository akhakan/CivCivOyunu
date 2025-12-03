# Unity Kamera Tabanlı Hareket -- Sayısal Analizli Açıklama

## 1. Kodun Satır Satır Açıklaması

### `[SerializeField] Transform _playerTransform;`

Inspector'dan atanabilen bir `Transform`.\
→ Oyuncunun pozisyon ve rotasyon bilgisi.

### `[SerializeField] Transform _orientationTransform;`

Inspector'dan atanabilen bir `Transform`.\
→ **Kameraya göre ileri yönü** belirleyen boş obje.\
→ Karakterin hareket edeceği yön: `orientation.forward`.

### `[SerializeField] Transform _playerVisualTransform;`

Inspector'dan atanabilen bir `Transform`.\
→ Oyuncunun sadece görsel modelidir.\
→ Hareket yönüne doğru döndürülür.

### `private float _rotationSpeed = 6f;`

→ Slerp **dönüş** hızını kontrol eder.

### `private float _verticalInput, _horizontalInput;`

→ Input değerlerini saklar.

### `Update()`

→ Kullacının W, A, S, D veya joystick girdilerini okur.

-   W → Vertical = +1
-   S → Vertical = -1
-   D → Horizontal = +1
-   A → Horizontal = -1

`GetAxisRaw()` → (-1 / 0 / +1) hızlı tepki verir.

------------------------------------------------------------------------

# 🎯 SAYISAL ÖRNEKLERLE KOD ÇÖZÜMÜ

### Varsayımlar:

**Player pozisyonu:**\
→ (5, 0, 5)

**Kamera pozisyonu:**\
→ (1, 3, 1)

**Input (W + D):**\
→ Vertical = 1\
→ Horizontal = 1

------------------------------------------------------------------------

## 🔵 1) `viewDirection` Hesaplama

Kod:
```csharp
_viewDirection =
    _playerTransform.position -
    new Vector3(transform.position.x, _playerTransform.position.y, transform.position.z);
```
Hesap:
```csharp
camera.y → player.y yapılır:

camera: (1, 3, 1)
camera.y = 0 yapılır → (1, 0, 1)
new Vector3( camera.x , player.y , camera.z ) → (1 , 0 , 1)

_viewDirection = (5,0,5) - (1,0,1)
               = (4,0,4)
```
Normalize:
```csharp
length = sqrt(4² + 4²) = 5.657
normalized = (0.707, 0, 0.707)
```
------------------------------------------------------------------------

## 🔵 2) `orientationTransform.forward`
```csharp
orientation.forward = (0.707, 0, 0.707)
```
------------------------------------------------------------------------

## 🔵 3) Input'a Göre Hareket Yönü

Kod:
```csharp
_inputDirection =
    orientation.forward * verticalInput +
    orientation.right   * horizontalInput;

forward = (0.707, 0, 0.707)\
right = (0.707, 0, -0.707)
```
Hesap:
```csharp
    forward * 1  = (0.707, 0, 0.707)
    right   * 1  = (0.707, 0, -0.707)
    ---------------------------------
    toplam       = (1.414, 0, 0)
```
Normalize → (1, 0, 0)
```csharp
→ W + D → karakter kameraya göre sağa gider.
```
------------------------------------------------------------------------

## 🔵 4) Player Görselinin Döndürülmesi

Kod:
```csharp
_playerVisualTransform.forward = Vector3.Slerp(
    _playerVisualTransform.forward,
    _inputDirection.normalized,
    Time.deltaTime * _rotationSpeed
);
```
rotation hız örneği:
```csharp
    rotationSpeed = 6
    deltaTime = 0.016
    Slerp factor = 0.096
```
→ Model her frame'de hedefe %9.6 yaklaşır.



## Time.deltaTime Nedir?

`Time.deltaTime`, bir önceki frame ile mevcut frame arasındaki süreyi **saniye cinsinden** verir.

Örneğin oyun 60 FPS (frame per second) ile çalışıyorsa:

```
deltaTime ≈ 1 / 60 ≈ 0.0167 saniye
```

Bu örnekte `deltaTime = 0.016` olarak alınmış. Yani her frame yaklaşık 0.016 saniye sürüyor.

---

## Slerp Faktörünün Hesabı

`Vector3.Slerp(a, b, t)` şu mantıkta çalışır:

> **Slerp sonucu** = a yönünden b yönüne doğru t oranında yaklaşır.

- `t` değeri 0 ile 1 arasında olmalıdır.
- Kodda `t = Time.deltaTime * _rotationSpeed` olarak verilmiş.

Örnek değerler:

```csharp
_rotationSpeed = 6
Time.deltaTime = 0.016
```

Hesap:

```
Slerp factor = deltaTime × rotationSpeed
             = 0.016 × 6
             = 0.096
```

Bu demek oluyor ki her frame model, hedef yönüne yaklaşık %9.6 yaklaşır.

---

## Mantık

- `rotationSpeed` değeri, **dönüşün ne kadar hızlı olacağını** belirler.
- `deltaTime` ile çarpılması, **FPS’den bağımsız bir dönüş hızı** sağlar.
- Sonuçta Slerp faktörü:

```
faktor = deltaTime × rotationSpeed
```

- Örnek: 0.096 → %9.6 yaklaşım  
- Eğer faktor 1 olursa, bir frame’de direkt hedef yönüne dönülür.