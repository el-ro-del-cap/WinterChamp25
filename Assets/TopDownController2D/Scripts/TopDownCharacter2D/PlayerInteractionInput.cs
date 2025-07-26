// PlayerInteractionInput.cs
using UnityEngine;
using UnityEngine.InputSystem; // Needed for InputValue
using UnityEngine.Events; // Needed for UnityEvent

// This script should be attached to the same GameObject as your Player Input component

public class PlayerInteractionInput : MonoBehaviour
{
    // This is the UnityEvent that other scripts (like InteractionAreaManager) will subscribe to
    private readonly UnityEvent onInteractEvent = new UnityEvent();
    public UnityEvent OnInteractEvent => onInteractEvent;

    // This method MUST match the name of your "Interact" Input Action
    // It will be automatically called by the Player Input component when behavior is "Send Messages"
    public void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("--- PlayerInteractionInput: OnInteract (E key) pressed! Invoking event. ---");
            onInteractEvent.Invoke();
        }
    }
}