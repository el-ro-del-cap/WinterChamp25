using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MouseMovement : MonoBehaviour
{
    public MinigameOverlord overlord;
    public float speed = 10f;
    private Vector3 mousePos;
    private Vector3 mouseOffset;
    public bool isMoving;
    public bool isAnchored;
    public Transform anchorPos;
    public float anchorOffset;
    public Transform wallPos;
    public float wallOffset;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isAnchored = false;
        isMoving = false;
        overlord = FindFirstObjectByType<MinigameOverlord>();
    }

    private void OnMouseDown()
    {
        mouseOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isMoving = true;
    }
    private void OnMouseUp()
    {
        isMoving = false;
        //if (isAnchored == true)
        //{
        //    transform.position = new Vector3(anchorPos.position.x, (anchorPos.position.y - anchorOffset));

        //}
    }
    private void OnMouseDrag()
    {
        //This handles movement only on the Y axis for the object.
        if (isMoving)
        {
            transform.position = new Vector3(-4.65f, mousePos.y) + new Vector3(0, mouseOffset.y);
        }

    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    #isMoving = false;
    //}

    // Update is called once per frame
    void Update()
    {
        //This one handles the plunger not leaving the toilet boundaries
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (isAnchored == true && transform.position.y < anchorOffset)
        {
            transform.position = new Vector3(anchorPos.position.x, (anchorPos.position.y - anchorOffset));
        }
        else if (isAnchored == true && transform.position.y > wallOffset)
        {
            transform.position = new Vector3(anchorPos.position.x, (wallPos.position.y - wallOffset));
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //when the plunger enters the anchor collider it enables the invisible ceiling and enters the anchored state
        //could also do it with SWITCH but what the hell it's just two tags
         if (collision.tag == "Anchor"){
            //isMoving = false;
            isAnchored = true;
            overlord.enableWall();
        }
        else if (collision.tag == "Wall" && overlord.plunges >= overlord.plungeGoal)
            {
            Debug.Log("Win");
            isMoving = false;
            GetComponent<BoxCollider2D>().enabled = false;
            overlord.winCondition();
        }

        

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Anchor")
            overlord.plunges++;
            Debug.Log("Score");
    }
}
