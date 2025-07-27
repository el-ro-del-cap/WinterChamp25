using UnityEngine;
using System.Collections.Generic;

public class CarryableItem : MonoBehaviour, IInteractable
{
    [Header("Interaction Settings")]
    public string interactionPrompt = "Press 'E' to pick up!";
    public float interactionRange = 2.0f;

    [Header("Visual Cue Settings")]
    [SerializeField]
    private GameObject arrowPrefab;
    public float arrowOffsetY = 1.0f;

    [Header("Item ID (unique for each item type)")]
    public string itemID;

    private PlayerInteractionInput playerInteractionInput;
    private TopDownCharacter2D.Controllers.TopDownInputController playerInputController;
    private bool playerInDetectionRange = false;
    private GameObject spawnedArrow;
    private Transform carriedBy;

    public string InteractionID => gameObject.name;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerInteractionInput = playerObject.GetComponent<PlayerInteractionInput>();
            playerInputController = playerObject.GetComponent<TopDownCharacter2D.Controllers.TopDownInputController>();
        }
        InitializeArrow();
        HideArrow();
    }

    private void InitializeArrow()
    {
        if (arrowPrefab != null)
        {
            spawnedArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity, transform);
            spawnedArrow.transform.localPosition = new Vector3(0, arrowOffsetY, 0);
            // Compensate for parent scale so world scale matches prefab
            Vector3 prefabWorldScale = arrowPrefab.transform.lossyScale;
            Vector3 parentLossy = spawnedArrow.transform.parent.lossyScale;
            spawnedArrow.transform.localScale = new Vector3(
                prefabWorldScale.x / parentLossy.x,
                prefabWorldScale.y / parentLossy.y,
                prefabWorldScale.z / parentLossy.z
            );
            spawnedArrow.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInteractionInput == null || playerInputController == null || spawnedArrow == null || carriedBy != null)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerInputController.transform.position);
        if (distanceToPlayer <= interactionRange)
        {
            if (!playerInDetectionRange)
            {
                playerInDetectionRange = true;
                ShowArrow();
                playerInteractionInput.OnInteractEvent.AddListener(AttemptInteraction);
            }
        }
        else
        {
            if (playerInDetectionRange)
            {
                playerInDetectionRange = false;
                HideArrow();
                playerInteractionInput.OnInteractEvent.RemoveListener(AttemptInteraction);
            }
        }
    }

    private void AttemptInteraction()
    {
        if (playerInDetectionRange && carriedBy == null)
        {
            AttachToPlayer();
        }
    }

    private void AttachToPlayer()
    {
        carriedBy = playerInputController.transform;
        HideArrow();
        CarryStackManager.Instance.AddToStack(this.gameObject);
    }

    private void ShowArrow()
    {
        if (spawnedArrow != null)
            spawnedArrow.SetActive(true);
    }

    private void HideArrow()
    {
        if (spawnedArrow != null)
            spawnedArrow.SetActive(false);
    }

    void OnDisable()
    {
        if (playerInteractionInput != null)
            playerInteractionInput.OnInteractEvent.RemoveListener(AttemptInteraction);
        if (spawnedArrow != null)
            Destroy(spawnedArrow);
    }

    public void Interact(GameObject interactor)
    {
        // Not used, handled by AttemptInteraction
    }

    public string GetInteractionPrompt()
    {
        return interactionPrompt;
    }
}
