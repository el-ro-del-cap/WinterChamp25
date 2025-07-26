using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TankController : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider flowSlider;
    public Image fillBar;
    public TMP_Text timerText;
    public TMP_Text percentageText;

    [Header("Tank Parameters")]
    public float tankCapacity; // Capacidad total de nafta
    public float timeLimit;    // Tiempo máximo para llenar el tanque
    public float maxFlowRate;  // Tasa de flujo máxima del slider
    public float minWinPercentage; // Porcentaje mínimo para ganar (ej. 90)
    public float maxWinPercentage; // Porcentaje máximo para ganar (ej. 110)

    private float currentFillAmount;
    private float currentTime;
    private bool gameActive = true;

    void Start()
    {
        // Configuración inicial
        currentTime = timeLimit;
        timerText.text = $"Tiempo: {currentTime:F1}";
        percentageText.text = "0%";
    }

    void Update()
    {
        if (!gameActive) return;

        currentTime -= Time.deltaTime;
        timerText.text = $"Tiempo: {currentTime:F1}";

        // Asegúrate de que el slider de flujo esté conectado
        if (flowSlider != null)
        {
            float flowRate = (flowSlider.value / flowSlider.maxValue) * maxFlowRate;
            currentFillAmount += flowRate * Time.deltaTime;

            // Actualizar la barra de llenado
            float fillPercentage = currentFillAmount / tankCapacity;
            fillBar.fillAmount = Mathf.Clamp(fillPercentage, 0, 1);
            percentageText.text = $"{(fillPercentage * 100):F0}%";
        }

        // Comprobar condiciones de victoria/derrota
        if (currentTime <= 0 || currentFillAmount > tankCapacity * (maxWinPercentage / 100f) + 0.01f) // Pequeño margen para evitar errores de coma flotante
        {
            EndGame();
        }
    }

    void EndGame()
    {
        gameActive = false;

        // Lógica de victoria/derrota con los nuevos límites
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

        // Aquí puedes enviar un evento al GameManager para que limpie la escena
        // y empiece un nuevo juego, por ejemplo.
    }
}