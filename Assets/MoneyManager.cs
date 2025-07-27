using System.Collections;
using System.Collections.Generic;
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

    [Header("Coin Popup UI")]
    public GameObject coinPopupPrefab;
    public GameObject lossPopupPrefab; // Nuevo campo para popup de pérdida
    public Transform coinPopupLayoutParent;
    public int coinPopupMaxCount = 7;
    public float coinPopupDuration = 3f;

    private List<GameObject> activeCoinPopups = new List<GameObject>();
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
            DeductExpensesWithPopup();
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

        // Show popup if credits were added
        if (amount > 0)
        {
            SpawnCoinPopup(amount);
        }
    }

    private void SpawnCoinPopup(int amount)
    {
        if (coinPopupLayoutParent == null) return;
        GameObject prefabToUse = amount < 0 && lossPopupPrefab != null ? lossPopupPrefab : coinPopupPrefab;
        if (prefabToUse == null) return;
        GameObject popup = GameObject.Instantiate(prefabToUse, coinPopupLayoutParent);
        var text = popup.GetComponentInChildren<TMPro.TMP_Text>();
        if (text != null)
            text.text = amount > 0 ? $"+{amount}" : $"{amount}";
        activeCoinPopups.Add(popup);
        if (activeCoinPopups.Count > coinPopupMaxCount)
        {
            GameObject.Destroy(activeCoinPopups[0]);
            activeCoinPopups.RemoveAt(0);
        }
        StartCoroutine(RemoveCoinPopupAfterDelay(popup, coinPopupDuration));
    }

    private IEnumerator RemoveCoinPopupAfterDelay(GameObject popup, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (activeCoinPopups.Contains(popup))
        {
            activeCoinPopups.Remove(popup);
            GameObject.Destroy(popup);
        }
    }

    public void DeductExpensesWithPopup()
    {
        credits -= lightExpenses;
        if (credits < 0)
        {
            credits = 0;
        }
        Debug.Log("Gastos de luz!");
        lastDeductedDay = gameTime.currentDay;
        UpdateCreditsText();
        // Mostrar popup negativo
        SpawnCoinPopup(-lightExpenses);
    }

    private void UpdateCreditsText()
    {
        creditsText.text = $"{credits}/{maxCredits}";
    }
}