using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class toiletOverlord : MonoBehaviour
{
    private AudioSource a_pop;
    private MinigameOverlord gameLord;
    private MouseMovement playerPlunge;
    public int plunges;
    public int plungeGoal;
    public GameObject invisibleWall;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        a_pop = GetComponent<AudioSource>();
        gameLord = FindFirstObjectByType<MinigameOverlord>();
        playerPlunge = GetComponentInChildren<MouseMovement>();
        plungeGoal = Random.Range(2, 4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updateCount()
    {
        gameLord.uncloggedCount++;
        a_pop.Play();
        if (gameLord.uncloggedCount == gameLord.uncloggedGoal)
            gameLord.winCondition();
    }
    public void enableWall()
    {
        invisibleWall.SetActive(true);
    }
}
