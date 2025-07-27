using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text creditsText;

    [Header("Money Parameters")]
    public int credits = 0;
    public int maxCredits = 10000;
    public int lightExpenses = 2000;

    [Header("Game Time Reference")]
    public GameTime gameTime;

    private int lastDeductedDay = 0;

    void Start()
    {
        // Asegurarse de que el texto se muestre correctamente al inicio
        UpdateCreditsText();
    }

    void Update()
    {
        // Verificar si han pasado 5 días y no hemos deducido los gastos todavía en este día
        if (gameTime != null && gameTime.currentDay % 5 == 0 && gameTime.currentDay != lastDeductedDay)
        {
            DeductExpenses();
        }
    }

    public void SumarCreditos(int amount)
    {
        credits += amount;

        // Limitar los créditos al máximo
        if (credits > maxCredits)
        {
            credits = maxCredits;
        }

        // Comprobar si se alcanzó el límite de 10000 y avisar
        if (credits == maxCredits)
        {
            Debug.Log("Bien ahi toda esa guitaaa");
        }

        print(credits);
        UpdateCreditsText();
    }

    private void DeductExpenses()
    {
        credits -= lightExpenses;

        // Evitar que los créditos bajen de 0
        if (credits < 0)
        {
            credits = 0;
        }

        Debug.Log("Gastos de luz!");
        lastDeductedDay = gameTime.currentDay; // Actualizar el día de la última deducción

        UpdateCreditsText();
    }

    private void UpdateCreditsText()
    {
        creditsText.text = $"Créditos: {credits}/{maxCredits}";
    }
}