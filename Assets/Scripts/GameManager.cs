using System;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Callback for minigame victory/end
    public Action OnGameEndedCallback;
    [Tooltip("Arrastra los prefabs de los tanques aqu�")]
    public GameObject[] tankPrefabs;
    public FuelGun fuelGun; // Referencia al script de la pistola

    [Tooltip("El objeto padre donde se instanciar� el tanque. Arrastra el objeto de la escena aqu�.")]
    public Transform spawnParent; // �Nuevo campo para el objeto padre!

    void Start()
    {
        StartNewGame();
    }

    // Este m�todo es llamado por el TankController cuando el juego termina
    public void OnGameEnded()
    {
        // Call the callback if set (for minigame manager)
        if (OnGameEndedCallback != null)
        {
			Debug.Log("Game ended, invoking callback.");
            var cb = OnGameEndedCallback;
            OnGameEndedCallback = null; // Clear before invoke to avoid reentrancy
            cb.Invoke();
            // Do NOT restart the game if callback was set (minigame context)
            return;
        }
    }

    private IEnumerator RestartGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartNewGame();
    }

    public void StartNewGame()
    {
        // Limpiar el objeto padre de cualquier tanque anterior
        if (spawnParent != null)
        {
            foreach (Transform child in spawnParent)
            {
                Destroy(child.gameObject);
            }
        }

        if (tankPrefabs.Length > 0 && spawnParent != null)
        {
            int randomTankIndex = UnityEngine.Random.Range(0, tankPrefabs.Length);

            // Instanciar el nuevo tanque como hijo del objeto padre
            GameObject newTankObject = Instantiate(tankPrefabs[randomTankIndex], spawnParent);

            // Opcional: Centrar el tanque dentro del padre si es necesario
            //newTankObject.transform.localPosition = Vector3.zero;

            TankController newTankController = newTankObject.GetComponent<TankController>();

            if (newTankController != null && fuelGun != null)
            {
                fuelGun.ConnectToTank(newTankController);
                newTankController.SetGameManager(this);
            }
            else
            {
                Debug.LogError("TankController o FuelGun no encontrados. Aseg�rate de que los scripts est�n adjuntos y las referencias configuradas.");
            }
        }
        else
        {
            Debug.LogError("No hay prefabs de tanques asignados o el objeto 'spawnParent' no est� configurado en el GameManager.");
        }
    }
}