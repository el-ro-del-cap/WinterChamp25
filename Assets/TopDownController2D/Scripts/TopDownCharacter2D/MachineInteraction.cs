// InteractionAreaManager.cs (Modified)
using UnityEngine;
// Remove 'using TopDownCharacter2D.Controllers;' as it's not directly needed for the event anymore
// using TopDownCharacter2D.Controllers; // Keep this if you still need playerInputController for other things like transform.position

public class InteractionAreaManager : MonoBehaviour, IInteractable
{
    [Header("Interaction Settings")]
    public string interactionPrompt = "Press 'E' to interact!";
    public float interactionRange = 2.0f; 
    public string interactionAreaID; 

    [Header("Visual Cue Settings")]
    [SerializeField]
    private GameObject arrowPrefab; 
    public float arrowOffsetY = 1.0f; 

    // CHANGE THIS: Now references the new PlayerInteractionInput script
    private PlayerInteractionInput playerInteractionInput; 
    
    // Keep this if you still need it for player's position, etc.
    private TopDownCharacter2D.Controllers.TopDownInputController playerInputController; 

    private bool playerInDetectionRange = false; 
    private GameObject spawnedArrow;

    public string InteractionID => interactionAreaID;

    public void Interact(GameObject interactor)
    {
        Debug.Log($"Player interacted with area: {gameObject.name} (ID: {interactionAreaID})");
        
        if (MiniGameManager.Instance != null)
        {
            MiniGameManager.Instance.StartMinigame(interactionAreaID);
        }
        else
        {
            Debug.LogError("MiniGameManager not found! Make sure it's in your scene.");
        }
    }

    public string GetInteractionPrompt()
    {
        return interactionPrompt;
    }

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player"); 
        if (playerObject != null)
        {
            // NEW: Get the PlayerInteractionInput component
            playerInteractionInput = playerObject.GetComponent<PlayerInteractionInput>(); 
            if (playerInteractionInput == null)
            {
                Debug.LogError("PlayerInteractionInput component not found on Player GameObject! Make sure it's attached.");
            }

            // Still get TopDownInputController if you need it for position/other movement data
            playerInputController = playerObject.GetComponent<TopDownCharacter2D.Controllers.TopDownInputController>();
            if (playerInputController == null)
            {
                Debug.LogError("TopDownInputController not found on Player GameObject! (for position check)");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found! Make sure your player has the tag 'Player'.");
        }

        InitializeArrow();
        HideArrow(); 
    }

    private void InitializeArrow()
    {
        if (arrowPrefab != null)
        {
            spawnedArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity, transform);
            spawnedArrow.transform.localPosition = new Vector3(arrowOffsetY, 0 , 0); // Corrected Y-offset
            spawnedArrow.SetActive(false); 
        }
        else
        {
            Debug.LogError("Arrow Prefab is not assigned in InteractionAreaManager for " + gameObject.name);
        }
    }

    void Update()
    {
        // Check for playerInteractionInput AND playerInputController for position
        if (playerInteractionInput == null || playerInputController == null || spawnedArrow == null)
        {
            // Debug.LogWarning("InteractionAreaManager Update: Critical references are null. Cannot proceed.");
            return; 
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerInputController.transform.position);

        if (distanceToPlayer <= interactionRange)
        {
            if (!playerInDetectionRange)
            {
                playerInDetectionRange = true;
                Debug.Log($"Player ENTERED detection range of {gameObject.name}. Showing arrow.");
                ShowArrow();
                // Subscribe to the event from the NEW PlayerInteractionInput script
                playerInteractionInput.OnInteractEvent.AddListener(AttemptInteraction); 
            }
        }
        else 
        {
            if (playerInDetectionRange) 
            {
                playerInDetectionRange = false;
                Debug.Log($"Player LEFT detection range of {gameObject.name}. Hiding arrow.");
                HideArrow();
                // Unsubscribe from the event from the NEW PlayerInteractionInput script
                playerInteractionInput.OnInteractEvent.RemoveListener(AttemptInteraction); 
            }
        }
    }

    private void AttemptInteraction()
    {
        Debug.Log($"--- AttemptInteraction called for {gameObject.name}! PlayerInDetectionRange: {playerInDetectionRange} ---");
        if (playerInDetectionRange)
        {
            Interact(playerInputController.gameObject); 
        }
    }

    private void ShowArrow()
    {
        if (spawnedArrow != null)
        {
            spawnedArrow.SetActive(true); 
        }
    }

    private void HideArrow()
    {
        if (spawnedArrow != null)
        {
            spawnedArrow.SetActive(false); 
        }
    }

    void OnDisable()
    {
        // Ensure unsubscribe from the NEW PlayerInteractionInput script
        if (playerInteractionInput != null)
        {
            playerInteractionInput.OnInteractEvent.RemoveListener(AttemptInteraction);
        }
        if (spawnedArrow != null)
        {
            Destroy(spawnedArrow);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange); 
    }
}