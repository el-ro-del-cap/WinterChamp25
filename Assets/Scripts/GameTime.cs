using UnityEngine;
using TMPro;

public class GameTime : MonoBehaviour
{
    [Header("UI Element")]
    public TMP_Text timeText;

    [Header("Time Parameters")]
    [Tooltip("Duración del día en segundos de tiempo real.")]
    public float dayDurationSeconds = 120f; // 2 minutos por día por defecto

    [Header("Initial Values")]
    public int startDay = 1;

    public int currentDay;
    private float dayTimer;
    private bool dayEnded = false;

    [Header("MoneyManager Reference")]
    public MoneyManager moneyManager;

    void Start()
    {
        currentDay = startDay;
        dayTimer = dayDurationSeconds;
        dayEnded = false;
        UpdateTimeText();
    }

    void Update()
    {
        if (!dayEnded)
        {
            dayTimer -= Time.deltaTime;
            if (dayTimer <= 0f)
            {
                dayTimer = 0f;
                EndDay();
            }
            UpdateTimeText();
        }
    }

    private void EndDay()
    {
        dayEnded = true;
        // Cobrar impuestos y mostrar popup
        if (moneyManager != null)
        {
            moneyManager.DeductExpensesWithPopup();
        }
        // Esperar un poco y luego empezar el siguiente día
        Invoke(nameof(StartNextDay), 2f);
    }

    private void StartNextDay()
    {
        currentDay++;
        dayTimer = dayDurationSeconds;
        dayEnded = false;
        UpdateTimeText();
    }

    private void UpdateTimeText()
    {
        int segundosRestantes = Mathf.CeilToInt(dayTimer);
        if (segundosRestantes < 0) segundosRestantes = 0;
        timeText.text = $"Día {currentDay} - {segundosRestantes} segundos del día";
    }
}