using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    public float ShakeForceMultiplier = 5f;
    public Rigidbody[] ShakingRigidbodies;

    public void RollDice()
    {
        foreach (var rb in ShakingRigidbodies)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.AddForce(Random.insideUnitSphere * ShakeForceMultiplier, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * ShakeForceMultiplier * 2f, ForceMode.Impulse);
        }
    }
}