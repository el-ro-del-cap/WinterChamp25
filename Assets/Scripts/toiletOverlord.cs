using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine.Audio;
using UnityEngine.ParticleSystemJobs;
using UnityEngine;

public class toiletOverlord : MonoBehaviour
{
    public ParticleSystem[] part;
    private AudioSource a_pop;
    public AudioClip[] a_clip_succ;
    public AudioClip[] a_clip_pop;
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
        a_pop.clip = a_clip_pop[Random.Range(0, 2)];
        a_pop.volume = 0.60f;
        gameLord.uncloggedCount++;
        a_pop.Play();
        part[1].Play();
        if (gameLord.uncloggedCount == gameLord.uncloggedGoal)
            gameLord.winCondition();
    }
    public void enableWall()
    {
        invisibleWall.SetActive(true);
    }

    public void plungePlus()
    {
        a_pop.clip = a_clip_succ[Random.Range(0, 6)];
        a_pop.volume = 0.30f;
        plunges++;
        Debug.Log("Score");
        a_pop.Play();
        part[0].Play();
    }
}
