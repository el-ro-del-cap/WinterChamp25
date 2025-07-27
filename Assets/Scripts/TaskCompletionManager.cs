using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TaskCompletionManager : MonoBehaviour
{
    public CarryStackManager carryStackManager;
    public MoneyManager moneyManager;
    public DialogBox dialogBox;
    public CustomerController customerController; // Assign in Inspector
    public int baseRewardPerItem = 500;
    public int discountPerExtraItem = 200;
    public Button completeTaskButton;

    void Start()
    {
        if (completeTaskButton != null)
            completeTaskButton.onClick.AddListener(CompleteTask);
    }

    public void CompleteTask()
    {
        if (carryStackManager == null || dialogBox == null || moneyManager == null)
        {
            Debug.LogError("TaskCompletionManager: Missing references!");
            return;
        }
        var currentTask = dialogBox.GetCurrentTask();
        if (currentTask == null)
        {
            Debug.LogWarning("No active task to complete.");
            return;
        }
        List<string> required = new List<string>(currentTask.requiredItemIDs);
        List<GameObject> carried = carryStackManager.GetCarriedItems();
        int delivered = 0;
        int extra = 0;
        List<GameObject> toRemove = new List<GameObject>();
        foreach (var item in carried)
        {
            var carryable = item.GetComponent<CarryableItem>();
            if (carryable != null && required.Contains(carryable.itemID))
            {
                delivered++;
                required.Remove(carryable.itemID); // Only count each required once
                toRemove.Add(item);
            }
            else if (carryable != null)
            {
                extra++;
                toRemove.Add(item);
            }
        }
        int reward = delivered * baseRewardPerItem - extra * discountPerExtraItem;
        if (reward < 0) reward = 0;
        // Remove delivered/extra items from stack
        foreach (var item in toRemove)
        {
            carryStackManager.RemoveFromStack(item);
            Destroy(item);
        }
        moneyManager.SumarCreditos(reward);
        Debug.Log($"Task complete! Delivered: {delivered}, Extra: {extra}, Reward: {reward}");
        // Optionally, clear the current task or show a summary

        // Notify customer controller to advance the customer loop
        if (customerController != null)
        {
            customerController.OnTaskCompleted();
        }
    }
}
