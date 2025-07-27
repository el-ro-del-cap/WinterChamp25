using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Tooltip("Arrastra los prefabs de los tanques aquí")]
    public GameObject[] tankPrefabs;
    public FuelGun fuelGun; // Referencia al script de la pistola

    [Tooltip("El objeto padre donde se instanciará el tanque. Arrastra el objeto de la escena aquí.")]
    public Transform spawnParent; // ¡Nuevo campo para el objeto padre!

    void Start()
    {
        StartNewGame();
    }

    // Este método es llamado por el TankController cuando el juego termina
    public void OnGameEnded()
    {
        StartCoroutine(RestartGameAfterDelay(3f));
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
            int randomTankIndex = Random.Range(0, tankPrefabs.Length);

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
                Debug.LogError("TankController o FuelGun no encontrados. Asegúrate de que los scripts están adjuntos y las referencias configuradas.");
            }
        }
        else
        {
            Debug.LogError("No hay prefabs de tanques asignados o el objeto 'spawnParent' no está configurado en el GameManager.");
        }
    }
}