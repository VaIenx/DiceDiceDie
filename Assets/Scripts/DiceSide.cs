using UnityEngine;

// Kommt auf jedes Side_X Child
// Das Child braucht ein weiteres leeres Child namens "FaceNormal"
// dessen Y-Achse (grüner Pfeil) senkrecht aus der Fläche zeigt
public class DiceSide : MonoBehaviour
{
    public int sideValue;          // Im Inspector setzen: 1, 2, 3 ...
    public Transform faceNormal;   // Das leere Child das aus der Fläche zeigt

    public float GetUpAlignment()
    {
        if (faceNormal == null)
        {
            Debug.LogWarning($"{name}: faceNormal nicht zugewiesen!");
            return -1f;
        }
        return Vector3.Dot(faceNormal.up, Vector3.up);
    }
}