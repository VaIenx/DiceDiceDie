using UnityEngine;
// Wichtig für das neue Input System!
using UnityEngine.InputSystem; 

[RequireComponent(typeof(PhysicsController))]
public class ShakeDetector : MonoBehaviour
{
    [Tooltip("Wie stark geschüttelt werden muss, um als 'aktiv' zu gelten")]
    public float ShakeDetectionThreshold = 2.0f;

    private float sqrThreshold;
    private PhysicsController physicsController;

    void Start()
    {
        sqrThreshold = Mathf.Pow(ShakeDetectionThreshold, 2);
        physicsController = GetComponent<PhysicsController>();

        // 1. Gyroskop im neuen Input System aktivieren
        if (UnityEngine.InputSystem.Gyroscope.current != null)
        {
            InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
        }

        // 2. Linearer Beschleunigungssensor aktivieren (Reine Bewegung ohne Schwerkraft)
        if (LinearAccelerationSensor.current != null)
        {
            InputSystem.EnableDevice(LinearAccelerationSensor.current);
        }
        else
        {
            Debug.LogWarning("LinearAccelerationSensor nicht hardwareseitig verfügbar.");
        }
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

        // Prio 1: Wir versuchen die reine Beschleunigung ohne Schwerkraft zu greifen
        if (LinearAccelerationSensor.current != null && LinearAccelerationSensor.current.enabled)
        {
            movement = LinearAccelerationSensor.current.acceleration.ReadValue();
        }
        // Fallback: Falls der Linear-Sensor fehlt, nutzen wir die Drehung des Gyroskops (voller Name!)
        else if (UnityEngine.InputSystem.Gyroscope.current != null && UnityEngine.InputSystem.Gyroscope.current.enabled)
        {
            movement = UnityEngine.InputSystem.Gyroscope.current.angularVelocity.ReadValue();
        }

        float sqrMag = movement.sqrMagnitude;

        // Wenn die Bewegung heftig genug ist
        if (sqrMag >= sqrThreshold)
        {
            float intensity = Mathf.Clamp(sqrMag / sqrThreshold, 1f, 3f);
            
            // Übergabe an deinen PhysicsController
            physicsController.ReceiveShakeInput(movement, intensity);
        }
    }
}