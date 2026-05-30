using UnityEngine;
using System.Collections;
using TMPro;

public class DiceResultReader : MonoBehaviour
{
    public DiceResult[] Dice;
    public TMP_Text ResultLabel;

    [Tooltip("Unter diesem Wert gilt ein Würfel als still")]
    public float SleepThreshold = 0.05f;

    private bool isWaiting = false;

    public void OnDiceRolled()
    {
        if (!isWaiting)
            StartCoroutine(WaitUntilStill());
    }

    private IEnumerator WaitUntilStill()
    {
        isWaiting = true;

        if (ResultLabel != null)
            ResultLabel.text = "...";

        yield return new WaitForSeconds(0.3f);

        bool allStill = false;
        while (!allStill)
        {
            allStill = true;
            foreach (var die in Dice)
            {
                Rigidbody rb = die.GetComponent<Rigidbody>();
                if (rb == null) continue;

                if (rb.linearVelocity.magnitude > SleepThreshold ||
                    rb.angularVelocity.magnitude > SleepThreshold)
                {
                    allStill = false;
                    break;
                }
            }
            yield return null;
        }

        // Addieren und Multiplizieren getrennt berechnen
        int addTotal = 0;
        int multiplyTotal = 1;
        bool hasMultiplier = false;
        string resultText = "";

        foreach (var die in Dice)
        {
            int value = die.GetResult();
            string opSymbol = die.Operation == DiceOperation.Multiply ? "×" : "+";
            resultText += $"{die.gameObject.name}: {opSymbol}{value}\n";

            if (die.Operation == DiceOperation.Multiply)
            {
                multiplyTotal *= value;
                hasMultiplier = true;
            }
            else
            {
                addTotal += value;
            }
        }

        int total = hasMultiplier ? addTotal * multiplyTotal : addTotal;
        resultText += $"Gesamt: {total}";

        if (ResultLabel != null)
            ResultLabel.text = total.ToString();

        Debug.Log(resultText);
        isWaiting = false;
    }
}