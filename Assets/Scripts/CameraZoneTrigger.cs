using UnityEngine;

public class CameraZoneTrigger : MonoBehaviour
{
    [Header("Camera to Move")] 
    public Camera targetCamera;

    [Header("Target Camera Position")] 
    public Vector3 cameraTargetPosition;
    public Vector3 cameraTargetRotation;
    public float moveDuration = 0.2f;

    [Header("Player Tag")]
    public string playerTag = "Player";

    private Coroutine moveCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && targetCamera != null)
        {
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveCamera(targetCamera, cameraTargetPosition, cameraTargetRotation, moveDuration));
        }
    }


    private System.Collections.IEnumerator MoveCamera(Camera cam, Vector3 targetPos, Vector3 targetRot, float duration)
    {
        Vector3 startPos = cam.transform.position;
        Quaternion startRot = cam.transform.rotation;
        Quaternion endRot = Quaternion.Euler(targetRot);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            cam.transform.position = Vector3.Lerp(startPos, targetPos, t);
            cam.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cam.transform.position = targetPos;
        cam.transform.rotation = endRot;
        moveCoroutine = null;
    }
}