using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch; 
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
// Diese Zeile löst den Namenskonflikt auf:
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

[RequireComponent(typeof(PhysicsController))]
public class KeyboardDiceTrigger : MonoBehaviour
{
    public DiceResultReader resultReader;
    private PhysicsController physicsController;

    // Zeitfenster in Sekunden, in dem der zweite Tippen erfolgen muss
    [SerializeField] private float doubleTapTimeWindow = 0.3f; 
    private float lastTapTime;

    void OnEnable()
    {
        // Aktiviert das Enhanced Touch System für Mobile
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        // Deaktiviert es wieder, wenn das Objekt inaktiv wird
        EnhancedTouchSupport.Disable();
    }

    void Start()
    {
        physicsController = GetComponent<PhysicsController>();
        
        // Falls der ResultReader nicht im Inspector zugewiesen wurde, versuchen wir ihn automatisch zu finden
        if (resultReader == null)
            resultReader = GetComponent<DiceResultReader>();
    }

    void Update()
    {
        // 1. PC: Überprüft, ob die Leertaste in diesem Frame gedrückt wurde
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TriggerDiceRoll();
            return; // Wenn PC-Input erkannt wurde, überspringen wir den Touch-Check für diesen Frame
        }

        // 2. HANDY: Überprüft Double-Tap auf dem Touchscreen
        CheckMobileDoubleTap();
    }

    private void CheckMobileDoubleTap()
    {
        // Schaut nach aktiven Berührungen auf dem Bildschirm
        if (Touch.activeTouches.Count > 0)
        {
            Touch currentTouch = Touch.activeTouches[0];

            // Begonnen bedeutet: Der Finger hat den Bildschirm gerade erst berührt
            if (currentTouch.phase == TouchPhase.Began)
            {
                // Berechnet die Zeitdifferenz zum letzten Tippen
                float timeSinceLastTap = Time.time - lastTapTime;

                if (timeSinceLastTap <= doubleTapTimeWindow)
                {
                    // Double-Tap erfolgreich!
                    TriggerDiceRoll();
                    // Setzt die Zeit zurück, damit ein dritter schneller Tap nicht sofort wieder auslöst
                    lastTapTime = 0f; 
                }
                else
                {
                    // Es war nur der erste Tap, wir merken uns die Zeit
                    lastTapTime = Time.time;
                }
            }
        }
    }

    private void TriggerDiceRoll()
    {
        physicsController.RollDice();
        
        if (resultReader != null)
        {
            resultReader.OnDiceRolled();
        }
        else
        {
            Debug.LogWarning("KeyboardDiceTrigger: Kein DiceResultReader gefunden/zugewiesen!");
        }
    }
}