using System.Collections.Generic;
using UnityEngine;



public class CustomerDialogManager : MonoBehaviour {
    public static CustomerDialogManager Instance { get; private set; }
    public TextAsset dialogJsonFile; // Assign CustomerDialogs.json in Inspector
    private Dictionary<string, CustomerDialogEntry> dialogDict;

    void Awake() {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        LoadDialogs();
    }

    private void LoadDialogs() {
        dialogDict = new Dictionary<string, CustomerDialogEntry>();
        if (dialogJsonFile == null) {
            Debug.LogError("CustomerDialogManager: No dialogJsonFile assigned!");
            return;
        }
        var wrapper = JsonUtility.FromJson<CustomerDialogList>("{\"customers\":" + dialogJsonFile.text + "}");
        foreach (var entry in wrapper.customers) {
            dialogDict[entry.customerId] = entry;
        }
    }

    public CustomerDialogEntry GetDialogForCustomer(string customerId) {
        if (dialogDict != null && dialogDict.ContainsKey(customerId))
            return dialogDict[customerId];
        Debug.LogWarning($"No dialog found for customerId: {customerId}");
        return null;
    }

    public CustomerDialog GetRandomDialog(string customerId) {
        var entry = GetDialogForCustomer(customerId);
        if (entry != null && entry.dialogs.Count > 0) {
            int idx = Random.Range(0, entry.dialogs.Count);
            return entry.dialogs[idx];
        }
        return null;
    }
}

[System.Serializable]
public class CustomerDialogEntry {
    public string customerId;
    public string name;
    public List<CustomerDialog> dialogs;
}

[System.Serializable]
public class CustomerDialog {
    public List<string> requestLines;
    public List<string> successLines;
}

[System.Serializable]
public class CustomerDialogList {
    public List<CustomerDialogEntry> customers;
}