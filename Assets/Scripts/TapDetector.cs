using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[RequireComponent(typeof(PhysicsController))]
public class TapDetector : MonoBehaviour
{
    [Tooltip("Maximale Zeit zwischen zwei Taps um als Doppeltap zu gelten (Sekunden)")]
    public float DoubleTapInterval = 0.3f;
    public DiceResultReader resultReader;

    private float lastTapTime = -1f;
    private PhysicsController physicsController;

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += OnFingerDown;
    }

    void OnDisable()
    {
        Touch.onFingerDown -= OnFingerDown;
        EnhancedTouchSupport.Disable();
    }

    void Start()
    {
        physicsController = GetComponent<PhysicsController>();
        if (resultReader == null)
            resultReader = GetComponent<DiceResultReader>();
    }

    private void OnFingerDown(Finger finger)
    {
        // Nur den ersten Finger auswerten
        if (finger.index != 0) return;

        float timeSinceLastTap = Time.unscaledTime - lastTapTime;

        if (lastTapTime > 0f && timeSinceLastTap <= DoubleTapInterval)
        {
            physicsController.RollDice();
            resultReader.OnDiceRolled();
            lastTapTime = -1f;
        }
        else
        {
            lastTapTime = Time.unscaledTime;
        }
    }
}