
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class CustomerController : MonoBehaviour {
    [Header("Dialog Integration")]
    public DialogBox dialogBox;
    // Called by TaskCompletionManager when a task is completed
        // Dialog progression state
    private List<string> currentDialogLines;
    private int currentDialogIndex;
    private bool dialogActive;
    private List<string> currentSuccessLines;
    private int currentSuccessIndex;
    private bool successDialogActive;
    public TaskGenerator taskGenerator;
    private bool pendingTaskAssignment = false;
    // Show the current dialog line in the dialog box
    public void OnTaskCompleted()
    {
        // Show success lines for this customer, then remove customer and spawn next
        if (currentCustomer != null)
        {
            var customerObj = currentCustomer.GetComponent<CustomerObjectScript>();
            string customerId = customerObj != null ? customerObj.customerId : currentCustomer.customerName;
            var dialogEntry = CustomerDialogManager.Instance.GetDialogForCustomer(customerId);
            if (dialogEntry != null)
            {
                var dialog = CustomerDialogManager.Instance.GetRandomDialog(customerId);
                if (dialog != null && dialogBox != null && dialog.successLines != null && dialog.successLines.Count > 0)
                {
                    currentSuccessLines = dialog.successLines;
                    currentSuccessIndex = 0;
                    successDialogActive = true;
                    // Ensure dialog box is visible
                    if (dialogBox.dialogPopupGroup != null)
                        dialogBox.dialogPopupGroup.SetActive(true);
                    else
                        dialogBox.gameObject.SetActive(true);
                    ShowCurrentSuccessLine();
                    return;
                }
            }
        }
        // If no success lines, just remove customer and spawn next
        RemoveAndSpawnNextCustomer();
    }

    private void ShowCurrentSuccessLine()
    {
        if (dialogBox != null && currentSuccessLines != null && currentSuccessIndex < currentSuccessLines.Count)
        {
            dialogBox.ShowDialogLines(new List<string> { currentSuccessLines[currentSuccessIndex] });
        }
    }

    private void RemoveAndSpawnNextCustomer()
    {
        if (currentCustomer != null)
        {
            CustomerLeaveScreen();
        }
        StartCoroutine(SpawnNextCustomerAfterDelay(carLeaveArriveTime));
    }

    private IEnumerator SpawnNextCustomerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SendNextCustomer();
        // Wait 2 seconds for arrival animation before dialog
        yield return new WaitForSeconds(2f);
        AssignDialogAndTaskToCurrentCustomer();
    }

    // Assign dialog and generate a task for the current customer
    private void AssignDialogAndTaskToCurrentCustomer()
    {
        if (currentCustomer != null)
        {
            var customerObj = currentCustomer.GetComponent<CustomerObjectScript>();
            string customerId = customerObj != null ? customerObj.customerId : currentCustomer.customerName;
            var dialogEntry = CustomerDialogManager.Instance.GetDialogForCustomer(customerId);
            if (dialogEntry != null)
            {
                var dialog = CustomerDialogManager.Instance.GetRandomDialog(customerId);
                if (dialog != null && dialogBox != null)
                {
                    // Set the face sprite for this customer
                    dialogBox.SetCustomerFaceById(customerId);
                    currentDialogLines = dialog.requestLines;
                    currentDialogIndex = 0;
                    dialogActive = true;
                    ShowCurrentDialogLine();
                    // Store dialog for task assignment after dialog
                    pendingTaskAssignment = true;
                }

   
                // Optionally: generate a new task for this customer here
                // If you have a TaskGenerator, call it here and pass the result to dialogBox.ShowTask(task)
            }
        }
    }

 private void ShowCurrentDialogLine()
    {
        if (dialogBox != null && currentDialogLines != null && currentDialogIndex < currentDialogLines.Count)
        {
            dialogBox.ShowDialogLines(new List<string> { currentDialogLines[currentDialogIndex] });
        }
    }

    public CustomerData[] customerData;

    public CustomerObjectScript currentCustomer;
    private int currentCustomerIndex;

    public Transform customerParent;
    public Transform customerStartPosition;
    public Transform customerTalkPosition;
    public Transform customerEndPosition;
    public float carLeaveArriveTime = 1.3f;


    List<int> waitingCustomers;

    private bool _canSendNextCustomer = true;

    public bool CanSendNextCustomer {
        get {
            return _canSendNextCustomer;
        }
    }



    private void PrepareWaitingCustomers() {
        List<int> finalList = new List<int>();
        for (int i = 0; i < customerData.Length; i++) {
            finalList.Add(i);
        }
        waitingCustomers = finalList;
    }

    [ContextMenu("Send Next Customer")]
    public void SendNextCustomer() {
        if (currentCustomer != null) {
            RemoveCustomer();
        }
        if (waitingCustomers == null || waitingCustomers.Count < 1) {
            PrepareWaitingCustomers();
        }
        int waitListIndex = Random.Range(0, waitingCustomers.Count);
        int customerIndex = waitingCustomers[waitListIndex];
        waitingCustomers.RemoveAt(waitListIndex);

        //Instanciar cliente
        StopTheMoveCR();
        currentCustomer = GameObject.Instantiate(customerData[customerIndex].prefab, customerParent);
        currentCustomer.transform.position = customerStartPosition.position;
        currentCustomerIndex = customerIndex;
        MoveCustomer(customerTalkPosition.position, carLeaveArriveTime, delegate {
            _canSendNextCustomer = true;
            AssignDialogAndTaskToCurrentCustomer();
        });
    }

    public void CustomerGreet() {
        int customerDialogueToPlay = customerData[currentCustomerIndex].lastDialoguePart + 1;
        if (customerDialogueToPlay >= currentCustomer.dialogos.Length) {
            //No hay dialogo
        } else {
            customerData[currentCustomerIndex].lastDialoguePart = customerDialogueToPlay;
            StartCoroutine(DoDialogueCR(currentCustomer.dialogos[customerDialogueToPlay].lineasEntrada));
        }

    }

    private IEnumerator DoDialogueCR(string[] dialogues) {
        int i = 0;
        bool displayingText = false;
        while (i < dialogues.Length) {
            if (!displayingText) {
                DisplayText(dialogues[i]);
            }
            if (Input.anyKeyDown) {
                displayingText = false;
                i++;
            }
            yield return null;
        }
        //Codigo que deje salir al usuario?
    }

    private void DisplayText(string textToDisplay) {
        Debug.Log("Muestra dialogo");
        //Code for displaying text here
    }

    public void DoLastCustomerLines() {
        if (currentCustomer != null) {
            StartCoroutine(DoFinalDialogueCR(currentCustomer.dialogos[customerData[currentCustomerIndex].lastDialoguePart].lineasFinales));
        }
    }

    private IEnumerator DoFinalDialogueCR(string[] dialogues) {
        int i = 0;
        bool displayingText = false;
        while (i < dialogues.Length) {
            if (!displayingText) {
                DisplayText(dialogues[i]);
            }
            if (Input.anyKeyDown) {
                displayingText = false;
                i++;
            }
            yield return null;
        }
        //Mandar proximo cliente
    }

    public void MoveCustomer(Vector3 targetPosition, float duration, EmptyDelegate callback) {
        StopTheMoveCR();
        moveCoroutine = StartCoroutine(MoveCustomerCR(targetPosition, duration, callback));
    }

    private void StopTheMoveCR() {
        if (moveCoroutine != null) {
            StopCoroutine(moveCoroutine);
        }
    }

    private Coroutine moveCoroutine;

    private IEnumerator MoveCustomerCR(Vector3 targetPosition, float duration, EmptyDelegate callback) {
        Vector3 startPosition = currentCustomer.transform.position;
        float startTime = Time.time;
        float endTime = startTime + duration;
        while (Time.time < endTime) {
            float lerpPoint = EasingsScript.EaseInOutBack(Mathf.InverseLerp(startTime, endTime, Time.time));
            if (currentCustomer == null) {
                yield break;
            }
            currentCustomer.transform.position = Vector3.LerpUnclamped(startPosition, targetPosition, lerpPoint);
            yield return null;
        }
        currentCustomer.transform.position = targetPosition;
        callback();

    }

    [ContextMenu("Bye Customer")]
    public void CustomerLeaveScreen() {
        MoveCustomer(customerEndPosition.position, carLeaveArriveTime, delegate {
            RemoveCustomer();
            _canSendNextCustomer = true;
        });
    }
    void Start() {
        SendNextCustomer();
    }
    private void RemoveCustomer() {
        Destroy(currentCustomer.gameObject);
        currentCustomer = null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update() {
        if (dialogActive && currentDialogLines != null)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                currentDialogIndex++;
                if (currentDialogIndex < currentDialogLines.Count)
                {
                    ShowCurrentDialogLine();
                }
                else
                {
                    dialogActive = false;
                    Debug.Log("Dialog finished for customer: " + (currentCustomer != null ? currentCustomer.customerName : "null"));
                    // Always assign a random task when dialog finishes
                    if (taskGenerator != null)
                    {
                        Debug.Log("Generating new task for customer: " + (currentCustomer != null ? currentCustomer.customerName : "null"));
                        taskGenerator.GenerateRandomTask();
                    }
                    pendingTaskAssignment = false;
                    if (dialogBox != null)
                        dialogBox.Hide();
                }
            }
        }
        else if (successDialogActive && currentSuccessLines != null)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                currentSuccessIndex++;
                if (currentSuccessIndex < currentSuccessLines.Count)
                {
                    ShowCurrentSuccessLine();
                }
                else
                {
                    successDialogActive = false;
                    if (dialogBox != null)
                        dialogBox.Hide();
                    RemoveAndSpawnNextCustomer();
                }
            }
        }
    }
    // (Stray else block removed)

}

public delegate void EmptyDelegate();