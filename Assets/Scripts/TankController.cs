using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TankController : MonoBehaviour
{
    [Header("UI Elements")]
    // Usamos RectTransform para manipular la escala
    public RectTransform fillBarRectTransform;
    public TMP_Text timerText;
    public TMP_Text percentageText;

    [Header("Tank Parameters")]
    public float tankCapacity;
    public float timeLimit;
    public float minWinPercentage;
    public float maxWinPercentage;

    private float currentFillAmount;
    private float currentTime;
    private bool gameActive = true;

    private GameManager gameManager;

    void Start()
    {
        currentTime = timeLimit;
        timerText.text = $"Tiempo: {currentTime:F1}";
        percentageText.text = "0%";

        // Al inicio, la escala en Y es 0
        if (fillBarRectTransform != null)
        {
            fillBarRectTransform.localScale = new Vector3(1, 0, 1);
        }
    }

    void Update()
    {
        if (!gameActive) return;

        currentTime -= Time.deltaTime;
        timerText.text = $"Tiempo: {currentTime:F1}";

        if (currentTime <= 0 || currentFillAmount > tankCapacity * (maxWinPercentage / 100f) + 0.01f)
        {
            EndGame();
        }
    }

    public void AddFuel(float amount)
    {
        if (!gameActive) return;

        // Aseguramos que la referencia no sea nula antes de usarla
        if (fillBarRectTransform == null)
        {
            Debug.LogError("fillBarRectTransform no está asignado. La imagen de nafta no se puede mover.");
            return;
        }

        currentFillAmount += amount;

        // Calcular el porcentaje de llenado (de 0 a 1)
        float fillPercentage = currentFillAmount / tankCapacity;

        // Ajustar la escala en el eje Y
        float newScaleY = Mathf.Clamp(fillPercentage, 0, 2);
        fillBarRectTransform.localScale = new Vector3(fillBarRectTransform.localScale.x, newScaleY, fillBarRectTransform.localScale.z);

        // Actualizar el texto del porcentaje
        percentageText.text = $"{(fillPercentage * 100):F0}%";
    }

    void EndGame()
    {
        gameActive = false;

        float minWinAmount = tankCapacity * (minWinPercentage / 100f);
        float maxWinAmount = tankCapacity * (maxWinPercentage / 100f);

        if (currentFillAmount >= minWinAmount && currentFillAmount <= maxWinAmount)
        {
            Debug.Log($"¡Victoria! Llenaste el tanque con {currentFillAmount:F1} de nafta, dentro de los límites de {minWinPercentage}% y {maxWinPercentage}%.");
        }
        else if (currentFillAmount > maxWinAmount)
        {
            Debug.Log($"¡Derrota! Te pasaste de nafta. Llenaste {currentFillAmount:F1}.");
        }
        else
        {
            Debug.Log($"¡Derrota! Te quedaste corto. Llenaste {currentFillAmount:F1}.");
        }

        if (gameManager != null)
        {
            gameManager.OnGameEnded();
        }
    }

    public void SetGameManager(GameManager manager)
    {
        gameManager = manager;
    }
}