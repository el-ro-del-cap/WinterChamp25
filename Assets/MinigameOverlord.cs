using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class MinigameOverlord : MonoBehaviour
{
    public MouseMovement playerPlunge;
    public int plunges;
    public int plungeGoal;
    public GameObject InvisibleWall;
    public GameObject winImg;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerPlunge = FindFirstObjectByType<MouseMovement>();
        plungeGoal = (Random.Range(4, 8));   
    }

    // Update is called once per frame
    void Update()
    {

    }  

    public void enableWall()
    {
        InvisibleWall.SetActive(true);
    }

    public void winCondition()
    {
        winImg.SetActive(true);
    }
}
