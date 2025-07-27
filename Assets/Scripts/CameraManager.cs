
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
    [System.Serializable]
    public class NamedObject
    {
        public string objectName;
        public GameObject obj;
    }

    public List<NamedCamera> cameras = new List<NamedCamera>();
    public List<NamedObject> objects = new List<NamedObject>();

    private ObjectOrCamera current;
    private Stack<ObjectOrCamera> stack = new Stack<ObjectOrCamera>();

    private struct ObjectOrCamera {
        public Camera cam;
        public GameObject obj;
        public bool IsCamera => cam != null;
        public bool IsObject => obj != null;
        public ObjectOrCamera(Camera c) { cam = c; obj = null; }
        public ObjectOrCamera(GameObject o) { obj = o; cam = null; }
    }

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
    }

    public void SwitchTo(string name)
    {
        // Try cameras first
        foreach (var namedCam in cameras)
        {
            if (namedCam.cameraName == name)
            {
                if (current.IsCamera)
                {
                    current.cam.enabled = false;
                    stack.Push(current);
                }
                else if (current.IsObject)
                {
                    current.obj.SetActive(false);
                    stack.Push(current);
                }
                namedCam.camera.enabled = true;
                current = new ObjectOrCamera(namedCam.camera);
            }
            else
            {
                namedCam.camera.enabled = false;
            }
        }
        // Try objects
        foreach (var namedObj in objects)
        {
            if (namedObj.objectName == name)
            {
                if (current.IsCamera)
                {
                    current.cam.enabled = false;
                    stack.Push(current);
                }
                else if (current.IsObject)
                {
                    current.obj.SetActive(false);
                    stack.Push(current);
                }
                namedObj.obj.SetActive(true);
                current = new ObjectOrCamera(namedObj.obj);
            }
            else if (namedObj.obj != null)
            {
                namedObj.obj.SetActive(false);
            }
        }
    }
    public void ShowObject(string name)
    {
        foreach (var namedObj in objects)
        {
            if (namedObj.objectName == name && namedObj.obj != null)
                namedObj.obj.SetActive(true);
            else if (namedObj.obj != null)
                namedObj.obj.SetActive(false);
        }
    }
	public void SwitchBack()
	{
		if (stack.Count > 0)
		{
			// Disable current
			if (current.IsCamera && current.cam != null)
				current.cam.enabled = false;
			else if (current.IsObject && current.obj != null)
				current.obj.SetActive(false);

			// Pop and enable previous
			current = stack.Pop();
			if (current.IsCamera && current.cam != null)
				current.cam.enabled = true;
			else if (current.IsObject && current.obj != null)
				current.obj.SetActive(true);
		}
	}
}
