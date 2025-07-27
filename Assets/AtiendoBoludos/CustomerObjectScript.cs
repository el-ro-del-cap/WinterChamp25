using UnityEngine;


public class CustomerObjectScript : MonoBehaviour {

    [Header("Dialog System")]
    public string customerId; // Set this in the Inspector to match the JSON

    public string customerName;
    public ParteDialogo[] dialogos;
}

[System.Serializable]
public class ParteDialogo {
    public string[] lineasEntrada;
    public string[] lineasFinales;
}
