using UnityEngine;
using System.Collections.Generic;

// Einmalig ausführen: Rechtsklick auf die Komponente im Inspector → "Setup Face Normals"
// Erstellt automatisch FaceNormal-Objekte für alle Side_X Children
public class DiceAutoSetup : MonoBehaviour
{
    [ContextMenu("Setup Face Normals")]
    public void SetupFaceNormals()
    {
        DiceSide[] sides = GetComponentsInChildren<DiceSide>();

        if (sides.Length == 0)
        {
            Debug.LogWarning("Keine DiceSide-Komponenten gefunden. Bitte zuerst DiceSide auf alle Side_X Children ziehen und sideValue setzen.");
            return;
        }

        MeshFilter meshFilter = GetComponentInChildren<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogWarning("Kein MeshFilter gefunden!");
            return;
        }

        Mesh mesh = meshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        int[] triangles = mesh.triangles;

        // Mittelpunkt des Würfels
        Vector3 center = meshFilter.transform.InverseTransformPoint(transform.position);

        // Für jede Seite: nächste Fläche im Mesh finden
        foreach (var side in sides)
        {
            // Altes FaceNormal löschen falls vorhanden
            Transform existing = side.transform.Find("FaceNormal");
            if (existing != null)
                DestroyImmediate(existing.gameObject);

            // Position des Side-Objekts im lokalen Raum des Mesh
            Vector3 sideLocalPos = meshFilter.transform.InverseTransformPoint(side.transform.position);

            // Nächste Dreiecksfläche zur Side-Position finden
            Vector3 bestNormal = Vector3.up;
            float bestDist = float.MaxValue;

            for (int i = 0; i < triangles.Length; i += 3)
            {
                Vector3 v0 = vertices[triangles[i]];
                Vector3 v1 = vertices[triangles[i + 1]];
                Vector3 v2 = vertices[triangles[i + 2]];

                Vector3 triCenter = (v0 + v1 + v2) / 3f;
                float dist = Vector3.Distance(triCenter, sideLocalPos);

                if (dist < bestDist)
                {
                    bestDist = dist;
                    // Normale aus Kreuzprodukt berechnen
                    bestNormal = Vector3.Cross(v1 - v0, v2 - v0).normalized;

                    // Normale muss vom Würfelmittelpunkt weg zeigen
                    if (Vector3.Dot(bestNormal, triCenter - center) < 0)
                        bestNormal = -bestNormal;
                }
            }

            // FaceNormal GameObject erstellen
            GameObject faceNormalObj = new GameObject("FaceNormal");
            faceNormalObj.transform.SetParent(side.transform);
            faceNormalObj.transform.localPosition = Vector3.zero;

            // Rotation so setzen dass Y-Achse in Richtung der Normalen zeigt
            Vector3 worldNormal = meshFilter.transform.TransformDirection(bestNormal);
            faceNormalObj.transform.rotation = Quaternion.FromToRotation(Vector3.up, worldNormal);

            // Dem DiceSide zuweisen
            side.faceNormal = faceNormalObj.transform;

            Debug.Log($"{side.name} (Side {side.sideValue}): Normale = {worldNormal:F2}");
        }

        Debug.Log($"Setup abgeschlossen: {sides.Length} Seiten konfiguriert.");
    }
}