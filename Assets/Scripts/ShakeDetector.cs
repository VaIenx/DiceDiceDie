using UnityEngine;

[RequireComponent(typeof(PhysicsController))]
public class ShakeDetector : MonoBehaviour
{
    [Tooltip("Wie stark geschüttelt werden muss um als 'aktiv' zu gelten")]
    public float ShakeDetectionThreshold = 2.0f;

    private float sqrThreshold;
    private PhysicsController physicsController;

    void Start()
    {
        sqrThreshold = Mathf.Pow(ShakeDetectionThreshold, 2);
        physicsController = GetComponent<PhysicsController>();

        // Gyroskop aktivieren
        Input.gyro.enabled = true;
    }

    void Update()
    {
        // Nur die echte Handbewegung, ohne Schwerkraftanteil
        Vector3 movement = Input.gyro.userAcceleration;
        float sqrMag = movement.sqrMagnitude;

        if (sqrMag >= sqrThreshold)
        {
            float intensity = Mathf.Clamp(sqrMag / sqrThreshold, 1f, 3f);
            physicsController.ApplyShake(movement, intensity);
        }
    }
}