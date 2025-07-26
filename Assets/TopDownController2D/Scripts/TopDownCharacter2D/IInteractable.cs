// IInteractable.cs
using UnityEngine;

public interface IInteractable
{
    // Method to call when the player interacts with this object
    void Interact(GameObject interactor); 

    // Optional: Method to get interaction prompt text
    string GetInteractionPrompt(); 

    // Optional: Property to get the interaction ID/type
    string InteractionID { get; } 
}