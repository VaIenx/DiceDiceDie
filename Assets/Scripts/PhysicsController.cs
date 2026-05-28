using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    public float ShakeForceMultiplier = 5f;
    public Rigidbody[] ShakingRigidbodies;

    private Vector3 currentMovement;
    private float currentIntensity;
    private bool isShaking;

    // Wird vom ShakeDetector aufgerufen, um Daten zu liefern
    public void ReceiveShakeInput(Vector3 movement, float intensity)
    {
        currentMovement = movement;
        currentIntensity = intensity;
        isShaking = true;
    }

    // Physik-Berechnungen IMMER in FixedUpdate für den Build
    void FixedUpdate()
    {
        if (isShaking)
        {
            foreach (var rb in ShakingRigidbodies)
            {
                if (rb != null)
                {
                    // Kraft und Drehung auf die Würfel anwenden
                    rb.AddForce(currentMovement * ShakeForceMultiplier * currentIntensity, ForceMode.Force);
                    rb.AddTorque(Random.insideUnitSphere * ShakeForceMultiplier * currentIntensity * 2f, ForceMode.Force);
                }
            }

            // Nach dem Frame zurücksetzen, damit es nicht endlos weiterwackelt
            isShaking = false;
        }
    }
}