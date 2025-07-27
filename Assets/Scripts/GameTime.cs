using UnityEngine;
using TMPro;

public class GameTime : MonoBehaviour
{
    [Header("UI Element")]
    public TMP_Text timeText;

    [Header("Time Parameters")]
    [Tooltip("Velocidad de avance de los minutos por segundo en tiempo real.")]
    public float minutesPerSecond = 5f;

    [Header("Initial Values")]
    public int startDay = 1;
    public int startHour = 9;
    public int startMinute = 0;

    public int currentDay;
    private int currentHour;
    private int currentMinute;
    private float minuteTimer;

    void Start()
    {
        // Inicializar las variables con los valores de inicio
        currentDay = startDay;
        currentHour = startHour;
        currentMinute = startMinute;
        minuteTimer = 0f;

        // Mostrar la hora inicial
        UpdateTimeText();
    }

    void Update()
    {
        // Aumentar el temporizador
        minuteTimer += Time.deltaTime * minutesPerSecond;

        // Comprobar si ha pasado un minuto completo
        if (minuteTimer >= 60f)
        {
            minuteTimer -= 60f; // Restar 60 para que el temporizador siga su curso
            AdvanceMinute();
        }
    }

    private void AdvanceMinute()
    {
        currentMinute++;

        // Si los minutos superan 59, avanzar la hora
        if (currentMinute > 59)
        {
            currentMinute = 0;
            currentHour++;

            // Si la hora supera las 17, avanzar el día y resetear la hora
            if (currentHour > 17)
            {
                currentHour = 9; // Volver a las 09:00
                currentDay++;

                Debug.Log("¡Pasó un día!");
            }
        }

        // Actualizar el texto en la UI
        UpdateTimeText();
    }

    private void UpdateTimeText()
    {
        // Formatear la hora y los minutos con dos dígitos (ej. 09 en vez de 9)
        string minuteString = currentMinute.ToString("00");
        string hourString = currentHour.ToString("00");

        timeText.text = $"Día {currentDay} - {hourString}:{minuteString}";
    }
}