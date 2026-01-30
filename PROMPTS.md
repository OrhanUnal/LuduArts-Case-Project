# LLM Kullanım Dokümantasyonu
## Özet

| Bilgi | Değer |
|-------|-------|
| Toplam prompt sayısı | 5 |
| Kullanılan araçlar | Claude
| En çok yardım alınan konular | Alternatif yol bulma |
| Tahmini LLM ile kazanılan süre | 2 - 2.5 saat 

---

## Prompt 1: Interact farklılıkları

**Araç:**  Claude
**Tarih/Saat:** 2026-01-30 14:41

**Prompt:**
```
i am creating a unity project with new input system and i need to assign 2 different input action to one key (one of them acts like a button and you need to hold the button for other one) but i need to handle both of them in the same function like
function(InputAction.CallbackContext context)
hold += function
button += function 
but i need to differ them. how can i do that with using context
```

**Alınan Cevap (Özet):**
```
Bana 3 tane method verdi bunlar context üzerinden id ve name kullanıyordu ancak magic number gibi direkt o anda neye eşit olması gerektiğini söylüyordu
```

**Nasıl Kullandım:**
- [ ] Direkt kullandım (değişiklik yapmadan)
- [X] Adapte ettim (değişiklikler yaparak)
- [ ] Reddettim (kullanmadım)

**Açıklama:**
> context üzerinden name kullanmak istemiyordum belki alternatif sunar diye sordum ama o da aklımda olan şekilde cevap verdi diye hızlıca o şekilde ilerledim

**Yapılan Değişiklikler (adapte ettiyseniz):**
> Onun bana verdiği halinde "if (context.action.name == "HoldAction")" birden fazla if bulunuyordu bunları tek bir yere toplayıp IInteractable içinde farklı fonksiyonlar çalıştırdım

---

## Prompt 2: Kapı kilit sistemi

**Araç:**  Claude
**Tarih/Saat:** 2026-01-30 15:21

**Prompt:**
```
using UnityEngine;
public class DoorScript : MonoBehaviour, IInteractable
{
    void IInteractable.InteractLogicButton()
    {
        Debug.Log("START OPENING DOOR BAR");
    }
    void IInteractable.InteractLogicHold()
    {
        Debug.Log("OPEN DOOR");
    }
}

i have a door prefab which is interactable and i want to open the door  after interactlogichold function and i want to have a serializefield bool which will check whether door is locked or not and if door is locked it should check players inventory for a specific key i want you to create a function for that logic and i did not make the inventory system yet so do you have any suggestion for that
```

**Alınan Cevap (Özet):**
```
Bana sıfırdan Kapı ve pick up key sistemi verdi ancak inventory sistemi vermedi
```

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [X] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Verdiği pick up key sistemini direkt kullandım ve o sisteme göre bir inventory sistemi ayarladım. Ardından kapı için vermiş olduğu kodun içinde animasyon için coroutine kullandığını fark ettim ve kendi coroutinlerimi benzer bir şekilde yazıp geri kalanını kullanmadan bıraktım

---

## Prompt 3: Kapı state machine hatası

**Araç:**  Claude
**Tarih/Saat:** 2026-01-30 15:51

**Prompt:**
```
using System.Collections;
using UnityEngine;
namespace InteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// Door interactable implementation.
    /// Supports toggle, hold-to-unlock and locked state logic.
    /// </summary>
    public class DoorInteractable : MonoBehaviour, IInteractable
    {
        #region Fields
        private enum stateOfDoor
        {
            Closed,
            InAnimation,
            Open
        }
        [SerializeField] private bool m_IsLocked = false;
        [SerializeField] private KeyScript.KeyTypes m_RequiredKeyType = KeyScript.KeyTypes.None;
        private const float k_AnimationDuration = 1.5f;
        private stateOfDoor m_CurrentState = stateOfDoor.Closed;
        #endregion
        #region Interface Implementations
        void IInteractable.InteractLogicButton()
        {
            if (m_CurrentState == stateOfDoor.InAnimation)
                return;
            if (m_IsLocked)
                Debug.Log("Door is locked. Hold to try unlocking.");
            else
                ToggleDoor();
        }
        void IInteractable.InteractLogicHold()
        {
            if (m_IsLocked)
            {
                TryUnlockDoor();
            }
            else
            {
                OpenDoor();
            }
        }
        #endregion
        #region Methods
        private void TryUnlockDoor()
        {
            if (InventoryManager.Instance.HasItem(m_RequiredKeyType))
            {
                m_IsLocked = false;
                Debug.Log("Door unlocked!");
                InventoryManager.Instance.RemoveItem(m_RequiredKeyType);
                OpenDoor();
            }
            else
            {
                Debug.Log($"You need {m_RequiredKeyType} to unlock this door.");
            }
        }
        private void OpenDoor()
        {
            if (m_CurrentState == stateOfDoor.Open)
                return;
            Debug.Log("DOOR OPENING");
            StartCoroutine(AnimateDoor(1));
            m_CurrentState = stateOfDoor.Open;
        }
        private void CloseDoor()
        {
            Debug.Log("DOOR CLOSING");
            StartCoroutine(AnimateDoor(-1));
            m_CurrentState = stateOfDoor.Closed;
        }
        private void ToggleDoor()
        {
            if (m_CurrentState == stateOfDoor.Open)
                CloseDoor();
            else
                OpenDoor();
        }
        private IEnumerator AnimateDoor(int direction)
        {
            m_CurrentState = stateOfDoor.InAnimation;
            Quaternion startRotation = transform.rotation;
            Quaternion endRotation =
                startRotation * Quaternion.Euler(0f, 90f * direction, 0f);
            float elapsedTime = 0f;
            while (elapsedTime < k_AnimationDuration)
            {
                transform.rotation = Quaternion.Slerp(
                    startRotation,
                    endRotation,
                    elapsedTime / k_AnimationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.rotation = endRotation;
        }
        #endregion
    }
}
in here i can see that state machine is not working correctly and i am assuming that because after starting coroutine which is asyncron, i am swapping state to closed or open and overriding the state in coroutine how can i fix it
```

**Alınan Cevap (Özet):**
```
Bana hatamı tahmin ettiğim gibi oluştuğunu ve coroutine içinde değiştirmek için parametre göndermem gerektiğini söyledi
```

**Nasıl Kullandım:**
- [X] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Ben aslında parametre göndermek istemediğim için başka bir yol vardır düşüncesiyle yazmıştım ancak sonrasında dediği gibi parametre gönderip coroutine içinde state değiştirdim

---

## Prompt 4: Kapı kilit sistemi

**Araç:**  Claude
**Tarih/Saat:** 2026-01-30 16:20

**Prompt:**
```
gonna create a new prefab object it will be a lever and when i interact with it i want it to create an event signal and i will connect doors to that signal and open every door with that event its pretty simple but i dont really have much time can u do it
```

**Alınan Cevap (Özet):**
```
Bana sıfırdan kapı ve lever sistemi verdi
```

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [ ] Adapte ettim
- [X] Reddettim

**Açıklama:**
> Uzun süreceği kanaatine vardım ve kendim sıfırdan implemente ettim

---

## Prompt 5: Kapı kilit sistemi

**Araç:**  Claude
**Tarih/Saat:** 2026-01-30 19:50

**Prompt:**
```
can i change the colors of the keys with respect to their current key type which is an enum
```

**Alınan Cevap (Özet):**
```
Bana OnValidate içinde bir switch case açıp materyali değiştirmemi söyledi
```

**Nasıl Kullandım:**
- [X] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> OnValidate içinde kullanmadım ancak hızlıca kopyalayıp yeni bir fonksiyon açıp onu da Start fonksiyonunda kullandım

## Genel Değerlendirme

### LLM'in En Çok Yardımcı Olduğu Alanlar
1. Aklımda olan yoldan farklı bir çözüm üretebilir miyim sorusuna cevap oldu
2. Debug konusunda yardımcı oldu

### LLM'in Yetersiz Kaldığı Alanlar
1. Zaman kazanmak için basit işleri verdim ama tüm kodu ve mimariyi bilmediği için yapamadı

### LLM Kullanımı Hakkında Düşüncelerim
> Büyük ihtimalle birkaç saat daha geç bitirirdim
> Eğer kod mimarisini biliyor olsaydı algoritmasını verip yapmasını istediğim kodları yaparak bana daha zaman kazandırırdı

---

*Bu şablon Ludu Arts Unity Intern Case için hazırlanmıştır.*
