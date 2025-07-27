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
        [SerializeField] private float moneyAmount;

        protected override void OnPickedUp(GameObject receiver)
        {
            //Pa subir la plata
        }
    }
}