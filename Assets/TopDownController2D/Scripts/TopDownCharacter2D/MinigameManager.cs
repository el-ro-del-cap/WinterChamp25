using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MiniGameManager : MonoBehaviour
{

	[Header("Minigame Prefabs")]
	public GameObject fuelMinigamePrefab;

	[Header("Money & Rewards")]
	public MoneyManager moneyManager;
	public int stratagemMinorWinReward = 200;
	public int toiletWinReward = 1000;
	public int fuelWinReward = 500;

	private GameObject activeMinigameInstance;

	public static MiniGameManager Instance { get; private set; } // Singleton pattern

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject); // Optional: keep manager across scenes
		}
	}

	private Coroutine minorWinCoroutine;

	// The interaction area that started the minigame (set by InteractionAreaManager)
	private InteractionAreaManager currentInteractionArea;

	public void StartMinigame(string miniGameID, InteractionAreaManager interactionArea = null)
	{
		Debug.Log($"MiniGameManager: Attempting to start mini-game with ID: {miniGameID}");

		var playerObj = GameObject.FindGameObjectWithTag("Player");
		TopDownCharacter2D.Controllers.TopDownInputController inputController = null;
		if (playerObj != null)
		{
			inputController = playerObj.GetComponent<TopDownCharacter2D.Controllers.TopDownInputController>();
		}

		// Track which interaction area started the minigame
		currentInteractionArea = interactionArea;

		int arrowVictories = interactionArea != null ? interactionArea.arrowGameVictories : 3;

		switch (miniGameID)
		{
			case "Arrows":
				Debug.Log("--- Starting Arrows Game ! ---");
				var stratagemGameObj = GameObject.Find("StratagemManager");
				if (stratagemGameObj != null)
				{
					var stratagemGame = stratagemGameObj.GetComponent<StratagemGame>();
					if (inputController != null)
					{
						inputController.SetMovementEnabled(false);
						stratagemGame.OnGameVictory.RemoveAllListeners();
						stratagemGame.OnGameVictory.AddListener(() => {
							inputController.SetMovementEnabled(true);
							if (minorWinCoroutine != null)
							{
								StopCoroutine(minorWinCoroutine);
								minorWinCoroutine = null;
							}
							// Give item reward if set
							if (currentInteractionArea != null)
							{
								currentInteractionArea.GiveRewardItemToPlayer();
							}
						});
					}
					if (minorWinCoroutine != null)
					{
						StopCoroutine(minorWinCoroutine);
					}
					minorWinCoroutine = StartCoroutine(HandleStratagemMinorWins(stratagemGame));
					stratagemGame.DoArrowsGame(arrowVictories);
				}
				else
				{
					Debug.LogError("StratagemGameManager GameObject not found in the scene!");
				}
				break;
			case "Toilet":
				Debug.Log("--- Opening Toilet Game! ---");
				skillStatic.Skill = 1;
				// Reinitialize the minigame before switching cameras
				var overlord = FindObjectOfType<MinigameOverlord>();
				if (overlord != null)
				{
					overlord.MinigameInit();
				}
				CameraManager.Instance.SwitchTo("ToiletMinigameCamera");
				if (inputController != null)
				{
					inputController.SetMovementEnabled(false);
				}
				if (overlord != null)
				{
					overlord.winImg.SetActive(false);
					overlord.OnWin = () =>
					{
						Debug.Log("--- Toilet Won! ---");
						moneyManager.SumarCreditos(toiletWinReward);
						if (currentInteractionArea != null)
						{
							currentInteractionArea.GiveRewardItemToPlayer();
						}
						if (inputController != null)
							inputController.SetMovementEnabled(true);
						CameraManager.Instance.SwitchTo("TopDownCamera");
					};
				}
				break;
			case "Fuel":
				Debug.Log("--- Starting Fuel Game! ---");
				if (fuelMinigamePrefab != null)
				{
					// You can instantiate or enable the fuel minigame here as needed
					// For now, just simulate a win for demonstration
					moneyManager.SumarCreditos(fuelWinReward);
					if (currentInteractionArea != null)
					{
						currentInteractionArea.GiveRewardItemToPlayer();
					}
					Debug.Log("Fuel minigame completed, reward given.");
				}
				else
				{
					Debug.LogError("FuelMinigamePrefab not assigned in MiniGameManager!");
				}
				break;
			default:
				Debug.LogWarning($"Mini-game ID '{miniGameID}' not recognized.");
				break;
		}

	}

	private IEnumerator HandleStratagemMinorWins(StratagemGame stratagemGame)
	{
		while (stratagemGame != null)
		{
			if (stratagemGame.minorVictory)
			{
				moneyManager.SumarCreditos(stratagemMinorWinReward);
			}
			yield return null;
		}
	}

}