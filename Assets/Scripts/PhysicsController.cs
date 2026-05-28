using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    public float ShakeForceMultiplier = 5f;
    public Rigidbody[] ShakingRigidbodies;

    public void ApplyShake(Vector3 movement, float intensity)
    {
        foreach (var rb in ShakingRigidbodies)
        {
            rb.AddForce(movement * ShakeForceMultiplier * intensity, ForceMode.Force);
            rb.AddTorque(Random.insideUnitSphere * ShakeForceMultiplier * intensity * 2f, ForceMode.Force);
        }
    }
}