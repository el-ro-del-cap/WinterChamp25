using UnityEngine;
using UnityEngine.UI;

public class FuelGun : MonoBehaviour
{
    public Slider flowSlider;
    public float maxFlowRate;

    private TankController currentTank;

    void Update()
    {
        if (currentTank != null)
        {
            // Calcula la tasa de flujo y la env�a al tanque actual
            float flowRate = (flowSlider.value / flowSlider.maxValue) * maxFlowRate;
            currentTank.AddFuel(flowRate * Time.deltaTime);
        }
    }

    // M�todo para conectar la pistola con un tanque espec�fico
    public void ConnectToTank(TankController tank)
    {
        currentTank = tank;
        // Reinicia el slider para el nuevo tanque
        if (flowSlider != null)
        {
            flowSlider.value = 0;
        }
    }
}