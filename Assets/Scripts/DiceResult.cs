using UnityEngine;

public enum DiceOperation { Add, Multiply }

// Kommt auf den Würfel selbst
public class DiceResult : MonoBehaviour
{
    public DiceOperation Operation = DiceOperation.Add;

    private DiceSide[] sides;

    void Awake()
    {
        sides = GetComponentsInChildren<DiceSide>();
    }

    public int GetResult()
    {
        DiceSide topSide = null;
        float highestDot = -1f;

        foreach (var side in sides)
        {
            float dot = side.GetUpAlignment();
            if (dot > highestDot)
            {
                highestDot = dot;
                topSide = side;
            }
        }

        if (topSide == null)
        {
            Debug.LogWarning($"{gameObject.name}: Keine Seite gefunden!");
            return -1;
        }

        return topSide.sideValue;
    }

    public void ResetResult() { }
}