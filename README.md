# Interaction System - [Orhan Ünal]

> Ludu Arts Unity Developer Intern Case

## Proje Bilgileri

| Unity Versiyonu | 6000.0.23f1 |
| Render Pipeline | Built-in / URP / HDRP |
| Case Süresi | 12 saat |

---

## Kurulum

1. Repository'yi klonlayın:
```bash
git clone https://github.com/OrhanUnal/LuduArts-Case-Project.git
```
2. Unity Hub'da projeyi açın
3. `Assets/Case/Scenes/SCN_TestScene.unity` sahnesini açın
4. Play tuşuna basın


## Nasıl Test Edilir

### Kontroller

| Tuş | Aksiyon |
|-----|---------|
| WASD | Hareket |
| Mouse | Bakış yönü |
| E | Etkileşim |
| Space | Zıplama |

### Test Senaryoları

1. **Door Test:**
   - Kapıya yaklaşın, "Press E to Open" mesajını görün
   - E'ye basın, eğer kapı kilitli değilse kapı açılsın
   - Tekrar basın, kapı kapansın

2. **Key + Locked Door Test:**
   - Kilitli kapıya yaklaşın, "Locked - Key Required" mesajını görün
   - Anahtarı bulun ve toplayın
   - Kilitli kapıya geri dönün, şimdi açılabilir olmalı
   - E' ye basılı tutun kapı açılsın

3. **Switch Test:**
   - Switch'e yaklaşın ve aktive edin
   - Kilitli olmayan tüm kapılar açılmış veya kapanmış olacak (açık olanlar kapanıp, kapalı olanlar açılmış olmalı) 

4. **Chest Test:**
   - Sandığa yaklaşın
   - E'ye basılı tutun, progress bar dolsun
   - Sandık açılsın
---

## Mimari Kararlar

### Interaction System Yapısı

Interactionlari playerdan cıkan bir Raycast ile tetikliyorum ve her obje kendi durumuna gore IInteractable Interface kodunda bulunan fonksiyonları overridelayıp ona göre tepki veriyor

**Neden bu yapıyı seçtim:**
> Her objenin farklı bir tepki vereceğini bildiğimden ötürü bunların hepsini bir Player kodunun içinde veya bir Manager kodunun içinde yapmaktansa objelerin kendi kendilerine karar vermesini istediğim için seçtim

**Alternatifler:**
> Objelerin kendi fonksiyonlarını override etmesi gerektiğinden zaten emindim ancak Interactı Player kodunun içinde değil de bir manager içinde gerçekleştirip sonra gereken objeyi çalıştırmayı düşündüm ancak her seferinde playerın konumunu ve kamerasının baktığı açıyı bir managerın içinden çekmektense direkt player içinde yapmayı daha uygun gördüm

**Trade-off'lar:**
> Kodun okunurluğu biraz düşüyor ancak hem implemente edişi daha kolay oldu hem de performans açısından olumlu etkiledi

### Kullanılan Design Patterns

| Pattern | Kullanım Yeri | Neden |
|---------|---------------|-------|
| [Observer] | [Event system] | [Açıklama] |
| [State] | [Door states] | [Açıklama] |
| [vb.] | | |

---

## Ludu Arts Standartlarına Uyum

### C# Coding Conventions

| Kural | Uygulandı | Notlar |
|-------|-----------|--------|
| m_ prefix (private fields) | [x] / [ ] | |
| s_ prefix (private static) | [x] / [ ] | |
| k_ prefix (private const) | [x] / [ ] | |
| Region kullanımı | [x] / [ ] | |
| Region sırası doğru | [x] / [ ] | |
| XML documentation | [x] / [ ] | |
| Silent bypass yok | [x] / [ ] | |
| Explicit interface impl. | [x] / [ ] | |

### Naming Convention

| Kural | Uygulandı | Örnekler |
|-------|-----------|----------|
| P_ prefix (Prefab) | [x] / [ ] | P_Door, P_Chest |
| M_ prefix (Material) | [x] / [ ] | M_Door_Wood |
| T_ prefix (Texture) | [x] / [ ] | |
| SO isimlendirme | [x] / [ ] | |

### Prefab Kuralları

| Kural | Uygulandı | Notlar |
|-------|-----------|--------|
| Transform (0,0,0) | [x] / [ ] | |
| Pivot bottom-center | [x] / [ ] | |
| Collider tercihi | [x] / [ ] | Box > Capsule > Mesh |
| Hierarchy yapısı | [x] / [ ] | |

### Zorlandığım Noktalar
> Prefab kuralları kısmında zorlandım anlamadığım nedenlerden ötürü sürekli konumları değişiyordu
> Dosyaları tekrar ayarlamak zorunda kaldım bazı isimleri yanlış girdim ve bazı şeyleri yanlış yere koyduğumu fark ettim 

---

## Tamamlanan Özellikler

### Zorunlu (Must Have)

- [x] / [ ] Core Interaction System
  - [x] / [ ] IInteractable interface
  - [x] / [ ] InteractionDetector
  - [x] / [ ] Range kontrolü

- [x] / [ ] Interaction Types
  - [x] / [ ] Instant
  - [x] / [ ] Hold
  - [x] / [ ] Toggle

- [x] / [ ] Interactable Objects
  - [x] / [ ] Door (locked/unlocked)
  - [x] / [ ] Key Pickup
  - [x] / [ ] Switch/Lever
  - [x] / [ ] Chest/Container

- [x] / [ ] UI Feedback
  - [x] / [ ] Interaction prompt
  - [x] / [ ] Dynamic text
  - [x] / [ ] Hold progress bar
  - [x] / [ ] Cannot interact feedback

- [x] / [ ] Simple Inventory
  - [x] / [ ] Key toplama
  - [x] / [ ] UI listesi

### Bonus (Nice to Have)

- [x] Animation entegrasyonu
- [ ] Sound effects
- [x] Multiple keys / color-coded
- [ ] Interaction highlight
- [ ] Save/Load states
- [x] Chained interactions

---

## Bilinen Limitasyonlar

### Tamamlanamayan Özellikler
1. Sound Effects - Uygun sesler bulamadım ve saçma seslerdense hiç olmamasını tercih ettim
2. Interaction Highlight - Materyel değiştirerek yapmayı denedim ama beklediğim sonucu vermedi
3. Save/Load States - Dürüst olmak gerekirse burada neyden bahsettiğinizi anlamadım ve çok üstünde durmadım

### Bilinen Bug'lar
1. [Hold Bar Cancelation] - Eger bir kapıyı veya chesti açmak için basılı tutarken bırakırsanız hold bar sıfırlanmak yerine tamamen dolup yok oluyor

### İyileştirme Önerileri
1. Ses dosyaları eklenebilirdi - Şuan ki haliyle oyun tamamen sessiz
2. Texture ve Materyal eklemeleri - Default Unity elementleri kullandim dolayisiyla daha iyi olabilir hepsi
3. Kamera açısını Y ekseninde değiştirme - bazen keyler çok aşağıda olduğu için alamıyorum ve kamera yukarı aşağı hareket etmiyor

---

## Dosya Yapısı

```
Assets/
├── Case/
│   ├── Scripts/
│       └──  Core/
│   │   │   ├── IInteractable.cs
│   │   │   └── InventoryManager.cs
│   │   ├── Interactables/
│   │   │   ├── DoorScript.cs
│   │   │   └── ChestScript.cs
|   |   |   └── KeyScript.cs
│   │   │   └── LeverScript.cs
│   │   ├── Player/
│   │   │   └── PlayerScript.cs
│   │   │   └── Player Controller
│   │   └── UI/
│   │   │   └── InteractionUiManager.cs
│   ├── Prefabs/
│   │   ├── Interactables/
│   │   │   ├── P_Door
│   │   │   ├── P_Chest
│   │   │   ├── P_Key
│   │   │   ├── P_Lever
│   │   ├── UI/
│   │   │   ├── UI
│   └── Scenes/
│       └── SCN_TestScene.unity
├── Docs/
│   ├── CSharp_Coding_Conventions.md
│   ├── Naming_Convention_Kilavuzu.md
│   └── Prefab_Asset_Kurallari.md
├── README.md
├── PROMPTS.md
└── .gitignore

## İletişim

| Bilgi | Değer |
|-------|-------|
| Ad Soyad | Orhan Ünal |
| E-posta | orhan.unal1@outlook.com |
| LinkedIn | https://www.linkedin.com/in/orhan-ünal-a1908829b/ |
| GitHub | https://github.com/OrhanUnal |

---

*Bu proje Ludu Arts Unity Developer Intern Case için hazırlanmıştır.*
