using UnityEngine;

public class PisteroManager : MonoBehaviour
{
    [Tooltip("Arrastra los prefabs de los tanques aquí")]
    public GameObject[] tankPrefabs;

    void Start()
    {
        StartNewGame();
    }

    public void StartNewGame()
    {
        // Limpiar la escena de cualquier tanque anterior
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Elegir un prefab de tanque al azar
        if (tankPrefabs.Length > 0)
        {
            int randomTankIndex = Random.Range(0, tankPrefabs.Length);
            GameObject newTank = Instantiate(tankPrefabs[randomTankIndex], transform.position, Quaternion.identity, transform);
            // El TankController del nuevo tanque se encargará de la lógica
        }
        else
        {
            Debug.LogError("No hay prefabs de tanques asignados en el GameManager.");
        }
    }
}