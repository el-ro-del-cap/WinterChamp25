using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 offset;
    private bool isMoving;
    public bool isAnchored;
    public Transform anchorPos;
    public float anchorOffset;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        isMoving = false;
    }

    private void OnMouseDown()
    {
        isMoving = true;
    }
    private void OnMouseUp()
    {
        isMoving = false;
        if (isAnchored == true)
        {
            transform.position = anchorPos.position - new Vector3(anchorPos.position.x, anchorPos.position.y);

        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    #isMoving = false;
    //}

    // Update is called once per frame
    void Update()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(offset);
        if (isMoving)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.tag == "Anchor"){

        }
    }

}
