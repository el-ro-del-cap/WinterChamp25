using UnityEngine;
using UnityEngine.UI;

public class FuelGun : MonoBehaviour
{
    public Slider flowSlider;
    public float maxFlowRate;
    public RectTransform pistol;

    // Referencia al componente AudioSource
    public AudioSource audioSource;
    // El sonido que se reproducirá en loop
    public AudioClip fuelSound;

    private TankController currentTank;
    private Quaternion initialRotation;
    private Quaternion maxRotation;

    void Start()
    {
        if (pistol != null)
        {
            pistol.localRotation = Quaternion.Euler(0, 0, -50);
            initialRotation = pistol.localRotation;
            maxRotation = initialRotation * Quaternion.Euler(0, 0, 90);
        }

        // Configurar el AudioSource
        if (audioSource != null && fuelSound != null)
        {
            audioSource.clip = fuelSound;
            audioSource.loop = true; // El sonido se reproducirá en bucle
            audioSource.playOnAwake = false; // No se reproducirá al inicio
        }
    }

    void Update()
    {
        if (currentTank != null)
        {
            float flowRate = (flowSlider.value / flowSlider.maxValue) * maxFlowRate;
            currentTank.AddFuel(flowRate * Time.deltaTime);

            float sliderValueNormalized = flowSlider.value / flowSlider.maxValue;
            pistol.localRotation = Quaternion.Slerp(initialRotation, maxRotation, sliderValueNormalized);

            // Lógica de audio
            if (audioSource != null)
            {
                if (flowSlider.value > 0)
                {
                    // Si el slider se está moviendo, reproduce el sonido y ajusta el pitch
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                    // Mapea el valor normalizado del slider (0-1) a un pitch de 1 a 3
                    audioSource.pitch = 1 + (sliderValueNormalized * 2);
                }
                else
                {
                    // Si el slider está en 0, detiene el sonido
                    if (audioSource.isPlaying)
                    {
                        audioSource.Stop();
                    }
                }
            }
        }
    }

    public void ConnectToTank(TankController tank)
    {
        currentTank = tank;

        if (flowSlider != null)
        {
            flowSlider.value = 0;
            pistol.localRotation = initialRotation;
        }

        // Asegúrate de detener el audio cuando cambies de tanque
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}