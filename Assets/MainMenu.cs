using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform transitionImage;
    [Tooltip("La imagen de la pantalla de cr�ditos")]
    public RectTransform creditsImage;
    [Tooltip("El punto final de la transici�n, generalmente el centro de la pantalla.")]
    public Vector3 targetPosition = Vector3.zero;

    [Header("Transition Settings")]
    [Tooltip("La velocidad a la que se mueve la imagen de transici�n.")]
    public float transitionSpeed = 5f;
    [Tooltip("El nombre de la escena a cargar al finalizar la transici�n.")]
    public string nextSceneName;

    // Guardaremos la posici�n original de la imagen de cr�ditos
    private Vector3 creditsOriginalPosition;

    void Start()
    {
        // Guardar la posici�n inicial de la imagen de cr�ditos
        if (creditsImage != null)
        {
            creditsOriginalPosition = creditsImage.localPosition;
        }
    }

    // Esta funci�n se llama al presionar el bot�n "New Game"
    public void StartGame()
    {
        StartCoroutine(TransitionAndLoadScene(transitionImage, targetPosition));
    }

    // Esta funci�n se llama al presionar el bot�n de "Cr�ditos"
    public void Credits()
    {
        StartCoroutine(TransitionToPosition(creditsImage, targetPosition));
    }

    // Esta funci�n se llama al presionar el bot�n "Atr�s"
    public void Back()
    {
        StartCoroutine(TransitionToPosition(creditsImage, creditsOriginalPosition));
    }

    // Corrutina gen�rica para transicionar una imagen a una posici�n final
    private IEnumerator TransitionToPosition(RectTransform imageToMove, Vector3 finalPosition)
    {
        // Mueve la imagen de forma suave
        while (Vector3.Distance(imageToMove.localPosition, finalPosition) > 0.1f)
        {
            imageToMove.localPosition = Vector3.Lerp(imageToMove.localPosition, finalPosition, Time.deltaTime * transitionSpeed);
            yield return null;
        }

        // Asegura que la imagen est� exactamente en la posici�n final
        imageToMove.localPosition = finalPosition;
    }

    // Corrutina para la transici�n principal del juego
    private IEnumerator TransitionAndLoadScene(RectTransform imageToMove, Vector3 finalPosition)
    {
        // Mueve la imagen de transici�n hacia el centro de la pantalla
        yield return StartCoroutine(TransitionToPosition(imageToMove, finalPosition));

        // Carga la nueva escena
        SceneManager.LoadScene(3);
    }
}