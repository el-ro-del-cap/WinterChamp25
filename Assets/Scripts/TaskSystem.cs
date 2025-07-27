using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TaskGenerator : MonoBehaviour
{
	[Header("Available Item IDs (fill with all possible itemIDs in your game)")]
	public List<string> availableItemIDs;
	[Header("Dialog options for clients")]
	public List<string> dialogOptions;
	[Header("Task size range (min/max items per task)")]
	public int minItems = 2;
	public int maxItems = 5;

	public Task currentTask;

	public DialogBox dialogBox;
	public float dialogShowSeconds = 10f;
	private float dialogTimer = 0f;
	private bool dialogActive = false;

	public void GenerateRandomTask()
	{
		currentTask = new Task();
		int groupSize = Random.Range(minItems, maxItems + 1);
		var pool = new List<string>(availableItemIDs);
		for (int i = 0; i < groupSize && pool.Count > 0; i++)
		{
			int idx = Random.Range(0, pool.Count);
			currentTask.requiredItemIDs.Add(pool[idx]);
			pool.RemoveAt(idx);
		}
		if (dialogOptions.Count > 0)
			currentTask.clientDialog = dialogOptions[Random.Range(0, dialogOptions.Count)];
		else
			currentTask.clientDialog = "Bring me these items!";
		if (dialogBox != null)
		{
			dialogBox.ShowTask(currentTask);
			dialogTimer = dialogShowSeconds;
			dialogActive = true;
		}
	}

	void Update()
	{
		if (dialogActive && dialogBox != null)
		{
			dialogTimer -= Time.deltaTime;
			if (dialogTimer <= 0f)
			{
				dialogBox.Hide();
				dialogActive = false;
			}
		}
		// Press O to generate a new task
		if (Input.GetKeyDown(KeyCode.O))
		{
			Debug.Log("Generating new task...");
			GenerateRandomTask();
		}
	}
}

public class Task
{
	public List<string> requiredItemIDs = new List<string>();
	public string clientDialog;
}
