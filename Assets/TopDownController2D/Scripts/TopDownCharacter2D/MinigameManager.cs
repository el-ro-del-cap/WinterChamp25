// MiniGameManager.cs
using UnityEngine;

public class MiniGameManager : MonoBehaviour{
    [Header("Minigame Prefabs")]
    public GameObject skibidiMinigamePrefab;
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

    public void StartMinigame(string miniGameID)
    {
        Debug.Log($"MiniGameManager: Attempting to start mini-game with ID: {miniGameID}");

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        TopDownCharacter2D.Controllers.TopDownInputController inputController = null;
        if (playerObj != null)
        {
            inputController = playerObj.GetComponent<TopDownCharacter2D.Controllers.TopDownInputController>();
        }

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
                        stratagemGame.OnGameVictory.AddListener(() => inputController.SetMovementEnabled(true));
                    }
                    stratagemGame.DoArrowsGame(3);
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
                // Switch to the toilet minigame camera using CameraManager
                CameraManager.Instance.SwitchTo("ToiletMinigameCamera");
                // Block player input
                if (inputController != null)
                {
                    inputController.SetMovementEnabled(false);
                }
                // Listen for minigame end (assume MinigameOverlord is in the scene)
                if (overlord != null)
                {
                    overlord.winImg.SetActive(false); // Hide win image at start
                    overlord.OnWin = () => {
						Debug.Log("--- Toilet Won! ---");
                        if (inputController != null)
							inputController.SetMovementEnabled(true);
                        CameraManager.Instance.SwitchTo("TopDownCamera");
                    };
                }
                break;
            default:
                Debug.LogWarning($"Mini-game ID '{miniGameID}' not recognized.");
                break;
        }
    }

}