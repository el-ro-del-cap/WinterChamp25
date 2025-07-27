
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

// Mapping for itemID to Sprite
[System.Serializable]
public class ItemSpriteMapping
{
    public string itemID;
    public Sprite sprite;
}

public class DialogBox : MonoBehaviour
{

    [Header("Dialog Popup Group (shows for 10s)")]
    public GameObject dialogPopupGroup;
    public TMP_Text dialogText;
    public Transform itemListParent;
    public GameObject itemIconPrefab; // Assign a prefab with an Image/Text for item icon/ID

    [Header("Current Task Layout (always visible)")]
    public Transform currentTaskItemListParent;

    [Header("Item Sprites")]
    public List<ItemSpriteMapping> itemSprites;
    public Sprite tickSprite;

    private Dictionary<string, Sprite> spriteDict;
    private Dictionary<string, GameObject> currentTaskIcons = new Dictionary<string, GameObject>();
    private Task currentTask;

    void Awake()
    {
        // Build the dictionary for fast lookup
        spriteDict = new Dictionary<string, Sprite>();
        Debug.Log($"DialogBox: Building spriteDict with {itemSprites.Count} entries");
        foreach (var mapping in itemSprites)
        {
            Debug.Log($"DialogBox: Adding itemID '{mapping.itemID}' with sprite '{(mapping.sprite != null ? mapping.sprite.name : "null")}'");
            if (!spriteDict.ContainsKey(mapping.itemID))
                spriteDict.Add(mapping.itemID, mapping.sprite);
        }
    }

    public void ShowTask(Task task)
    {
        currentTask = task;
        if (dialogText != null)
        {
            dialogText.text = task.clientDialog;
            Debug.Log($"DialogBox: Set dialog text to: {task.clientDialog}");
        }
        else
        {
            Debug.LogWarning("DialogBox: dialogText is not assigned!");
        }
        // Clear previous icons in popup
        if (itemListParent != null)
        {
            foreach (Transform child in itemListParent)
                Destroy(child.gameObject);
            // Add icons for each required item in popup
            foreach (string itemID in task.requiredItemIDs)
            {
                Debug.Log($"DialogBox: Requesting icon for itemID '{itemID}'");
                var icon = Instantiate(itemIconPrefab, itemListParent);
                var image = icon.GetComponentInChildren<UnityEngine.UI.Image>();
                if (image == null)
                {
                    Debug.LogWarning($"DialogBox: No Image component found in itemIconPrefab or its children!");
                }
                if (image != null && spriteDict != null && spriteDict.ContainsKey(itemID))
                {
                    Debug.Log($"DialogBox: Assigning sprite '{spriteDict[itemID]?.name}' to itemID '{itemID}'");
                    image.sprite = spriteDict[itemID];
                }
                else if (image != null)
                {
                    Debug.LogWarning($"DialogBox: No sprite found for itemID '{itemID}', using prefab's default sprite.");
                    image.sprite = null;
                }
                var text = icon.GetComponentInChildren<UnityEngine.UI.Text>();
                if (text != null)
                    text.text = itemID;
            }
        }
        else
        {
            Debug.LogWarning("DialogBox: itemListParent is not assigned!");
        }
        if (dialogPopupGroup != null)
        {
            dialogPopupGroup.SetActive(true);
            Debug.Log("DialogBox: Showing dialogPopupGroup");
        }
        else
        {
            gameObject.SetActive(true);
            Debug.Log("DialogBox: Showing DialogBox GameObject");
        }
        // Also update the persistent current task layout
        UpdateCurrentTaskLayout();
    }

    public void Hide()
    {
        if (dialogPopupGroup != null)
            dialogPopupGroup.SetActive(false);
        else
            gameObject.SetActive(false);
    }

    public void UpdateCurrentTaskLayout()
    {
        if (currentTaskItemListParent == null || currentTask == null) return;
        foreach (Transform child in currentTaskItemListParent)
            Destroy(child.gameObject);
        currentTaskIcons.Clear();
        foreach (string itemID in currentTask.requiredItemIDs)
        {
            var icon = Instantiate(itemIconPrefab, currentTaskItemListParent);
            var image = icon.GetComponentInChildren<UnityEngine.UI.Image>();
            if (image != null && spriteDict != null && spriteDict.ContainsKey(itemID))
                image.sprite = spriteDict[itemID];
            var text = icon.GetComponentInChildren<UnityEngine.UI.Text>();
            if (text != null)
                text.text = itemID;
            currentTaskIcons[itemID] = icon;
        }
    }

    public void MarkItemCollected(string itemID)
    {
        Debug.Log($"MarkItemCollected called with itemID: {itemID}");
        foreach (var key in currentTaskIcons.Keys)
            Debug.Log($"CurrentTaskIcon key: {key}");
        if (currentTaskIcons.ContainsKey(itemID))
        {
            var icon = currentTaskIcons[itemID];
            var image = icon.GetComponentInChildren<UnityEngine.UI.Image>();
            if (image != null && tickSprite != null)
                image.sprite = tickSprite;
        }
        else
        {
            Debug.LogWarning($"MarkItemCollected: itemID '{itemID}' not found in currentTaskIcons!");
        }
    }


}
