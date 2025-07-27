using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomerController : MonoBehaviour {

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
        });
    }

    public void CustomerGreet() {
        int customerDialogueToPlay = customerData[currentCustomerIndex].lastDialoguePart + 1;
        if (customerDialogueToPlay >= currentCustomer.dialogos.Length) {
            //No hay dialogo
        } else {
            customerData[currentCustomerIndex].lastDialoguePart = customerDialogueToPlay;
            DoDialogueCR(currentCustomer.dialogos[customerDialogueToPlay].lineasEntrada);
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

    private void RemoveCustomer() {
        Destroy(currentCustomer.gameObject);
        currentCustomer = null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

    }





}

public delegate void EmptyDelegate();