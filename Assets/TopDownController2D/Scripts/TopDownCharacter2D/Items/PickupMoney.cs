using TopDownCharacter2D.Health;
using UnityEngine;

namespace TopDownCharacter2D.Items
{
    /// <summary>
    ///     Handles the logic of a money pickup item
    /// </summary>
    public class PickupMoney : PickupItem
    {
        [Tooltip("The amount of money given when picked up")]
        [SerializeField] private int moneyAmount;
        private GameObject UIManager;
        private MoneyManager moneyManager;

        void Start()
        {
            moneyManager = GameObject.Find("MoneyManager").GetComponent<MoneyManager>();
        }

        protected override void OnPickedUp(GameObject receiver)
        {
            moneyManager.SumarCreditos(moneyAmount);
        }
    }
}