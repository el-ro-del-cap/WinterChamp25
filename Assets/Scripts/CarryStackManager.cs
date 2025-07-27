using UnityEngine;
using System.Collections.Generic;

public class CarryStackManager : MonoBehaviour
{
    public static CarryStackManager Instance { get; private set; }
    public float stackOffsetY = 1.0f;
    private List<GameObject> carriedItems = new List<GameObject>();
    private Transform playerTransform;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            playerTransform = playerObject.transform;
    }

    public void AddToStack(GameObject item)
    {
        carriedItems.Add(item);
        item.transform.SetParent(playerTransform);
        UpdateStackPositions();
    }

    private void UpdateStackPositions()
    {
        for (int i = 0; i < carriedItems.Count; i++)
        {
            carriedItems[i].transform.localPosition = new Vector3(0, stackOffsetY * (i + 1), 0);
        }
    }
}
