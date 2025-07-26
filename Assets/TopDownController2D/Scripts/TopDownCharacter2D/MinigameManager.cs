// MiniGameManager.cs
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
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

        switch (miniGameID)
        {
            case "Arrows":
                Debug.Log("--- Starting Arrows Game ! ---");
                // Activate the Stratagem game (ensure the GameObject is in the scene and initially inactive)
                var stratagemGameObj = GameObject.Find("StratagemManager");
                if (stratagemGameObj != null)
                {
                    var stratagemGame = stratagemGameObj.GetComponent<StratagemGame>();
                    // Disable player movement
                    var playerObj = GameObject.FindGameObjectWithTag("Player");
                    if (playerObj != null)
                    {
                        var inputController = playerObj.GetComponent<TopDownCharacter2D.Controllers.TopDownInputController>();
                        if (inputController != null)
                        {
                            inputController.SetMovementEnabled(false);
                            // Subscribe to re-enable movement when the game is won
                            stratagemGame.OnGameVictory.RemoveAllListeners();
                            stratagemGame.OnGameVictory.AddListener(() => inputController.SetMovementEnabled(true));
                        }
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
                // Activate shop UI, populate items, etc.
                break;
            default:
                Debug.LogWarning($"Mini-game ID '{miniGameID}' not recognized.");
                break;
        }
    }

}