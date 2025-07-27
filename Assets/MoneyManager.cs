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
        // Verificar si han pasado 5 d�as y no hemos deducido los gastos todav�a en este d�a
        if (gameTime != null && gameTime.currentDay % 5 == 0 && gameTime.currentDay != lastDeductedDay)
        {
            DeductExpenses();
        }
    }

    public void SumarCreditos(int amount)
    {
        credits += amount;

        // Limitar los cr�ditos al m�ximo
        if (credits > maxCredits)
        {
            credits = maxCredits;
        }

        // Comprobar si se alcanz� el l�mite de 10000 y avisar
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

        // Evitar que los cr�ditos bajen de 0
        if (credits < 0)
        {
            credits = 0;
        }

        Debug.Log("Gastos de luz!");
        lastDeductedDay = gameTime.currentDay; // Actualizar el d�a de la �ltima deducci�n

        UpdateCreditsText();
    }

    private void UpdateCreditsText()
    {
        creditsText.text = $"Cr�ditos: {credits}/{maxCredits}";
    }
}