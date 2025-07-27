using UnityEngine;


public class CustomerObjectScript : MonoBehaviour {

    public string customerName;
    public ParteDialogo[] dialogos;
}

[System.Serializable]
public class ParteDialogo {
    public string[] lineasEntrada;
    public string lineaFinal;
}
