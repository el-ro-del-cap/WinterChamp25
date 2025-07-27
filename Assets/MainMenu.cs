using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform transitionImage;
    [Tooltip("La imagen de la pantalla de créditos")]
    public RectTransform creditsImage;
    [Tooltip("El punto final de la transición, generalmente el centro de la pantalla.")]
    public Vector3 targetPosition = Vector3.zero;

    [Header("Transition Settings")]
    [Tooltip("La velocidad a la que se mueve la imagen de transición.")]
    public float transitionSpeed = 5f;
    [Tooltip("El nombre de la escena a cargar al finalizar la transición.")]
    public string nextSceneName;

    // Guardaremos la posición original de la imagen de créditos
    private Vector3 creditsOriginalPosition;

    void Start()
    {
        // Guardar la posición inicial de la imagen de créditos
        if (creditsImage != null)
        {
            creditsOriginalPosition = creditsImage.localPosition;
        }
    }

    // Esta función se llama al presionar el botón "New Game"
    public void StartGame()
    {
        StartCoroutine(TransitionAndLoadScene(transitionImage, targetPosition));
    }

    // Esta función se llama al presionar el botón de "Créditos"
    public void Credits()
    {
        StartCoroutine(TransitionToPosition(creditsImage, targetPosition));
    }

    // Esta función se llama al presionar el botón "Atrás"
    public void Back()
    {
        StartCoroutine(TransitionToPosition(creditsImage, creditsOriginalPosition));
    }

    // Corrutina genérica para transicionar una imagen a una posición final
    private IEnumerator TransitionToPosition(RectTransform imageToMove, Vector3 finalPosition)
    {
        // Mueve la imagen de forma suave
        while (Vector3.Distance(imageToMove.localPosition, finalPosition) > 0.1f)
        {
            imageToMove.localPosition = Vector3.Lerp(imageToMove.localPosition, finalPosition, Time.deltaTime * transitionSpeed);
            yield return null;
        }

        // Asegura que la imagen esté exactamente en la posición final
        imageToMove.localPosition = finalPosition;
    }

    // Corrutina para la transición principal del juego
    private IEnumerator TransitionAndLoadScene(RectTransform imageToMove, Vector3 finalPosition)
    {
        // Mueve la imagen de transición hacia el centro de la pantalla
        yield return StartCoroutine(TransitionToPosition(imageToMove, finalPosition));

        // Carga la nueva escena
        SceneManager.LoadScene(3);
    }
}