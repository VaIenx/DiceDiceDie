using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PhysicsController))]
public class DiceInputController : MonoBehaviour
{
    public DiceResultReader resultReader;
    private PhysicsController physicsController;

    void Start()
    {
        physicsController = GetComponent<PhysicsController>();
        
        // Falls der ResultReader nicht im Inspector zugewiesen wurde, versuchen wir ihn automatisch zu finden
        if (resultReader == null)
            resultReader = GetComponent<DiceResultReader>();
    }

    void Update()
    {
        // Überprüft, ob die Leertaste in diesem Frame gedrückt wurde
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TriggerDiceRoll();
        }
    }

    private void TriggerDiceRoll()
    {
        physicsController.RollDice();
        
        if (resultReader != null)
        {
            resultReader.OnDiceRolled();
        }
        else
        {
            Debug.LogWarning("KeyboardDiceTrigger: Kein DiceResultReader gefunden/zugewiesen!");
        }
    }
}