using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [System.Serializable]
    public class NamedCamera
    {
        public string cameraName;
        public Camera camera;
    }

    public List<NamedCamera> cameras = new List<NamedCamera>();
    private Camera currentCamera;
    private Stack<Camera> cameraStack = new Stack<Camera>();

    public static CameraManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // Disable all cameras except the first one
        for (int i = 0; i < cameras.Count; i++)
        {
            if (i == 0)
            {
                cameras[i].camera.enabled = true;
                currentCamera = cameras[i].camera;
            }
            else
            {
                cameras[i].camera.enabled = false;
            }
        }
    }

    public void SwitchTo(string cameraName)
    {
        foreach (var namedCam in cameras)
        {
            if (namedCam.cameraName == cameraName)
            {
                if (currentCamera != null)
                {
                    currentCamera.enabled = false;
                    cameraStack.Push(currentCamera);
                }
                namedCam.camera.enabled = true;
                currentCamera = namedCam.camera;
            }
            else
            {
                namedCam.camera.enabled = false;
            }
        }
    }

    public void SwitchBack()
    {
        if (cameraStack.Count > 0)
        {
            if (currentCamera != null)
                currentCamera.enabled = false;
            currentCamera = cameraStack.Pop();
            if (currentCamera != null)
                currentCamera.enabled = true;
        }
    }
}
