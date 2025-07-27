using UnityEngine;
using System.Collections.Generic;

public class CarryStackManager : MonoBehaviour
{
    public static CarryStackManager Instance { get; private set; }
    public float stackOffsetY = 1.0f;
    [Tooltip("The Y position (local) where the first item in the stack will be placed above the player.")]
    public float stackStartY = 0.0f;
    private List<GameObject> carriedItems = new List<GameObject>();
    private Transform playerTransform;

    [Header("UI Reference")]
    public DialogBox dialogBox;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            playerTransform = playerObject.transform;
    }

    public void AddToStack(GameObject item)
    {
        carriedItems.Add(item);
        item.transform.SetParent(playerTransform);
        UpdateStackPositions();

        // Mark as collected in dialog if item is part of the current task
        var carryable = item.GetComponent<CarryableItem>();
        if (carryable != null && dialogBox != null)
        {
            dialogBox.MarkItemCollected(carryable.itemID);
        }
    }

    private void UpdateStackPositions()
    {
        for (int i = 0; i < carriedItems.Count; i++)
        {
            // Always reset localPosition.x and z to 0, and y to stacking value
            Vector3 pos = carriedItems[i].transform.localPosition;
            pos.x = 0;
            pos.z = 0;
            pos.y = stackStartY + stackOffsetY * i;
            carriedItems[i].transform.localPosition = pos;
        }
    }
}
