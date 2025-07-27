using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinigameOverlord : MonoBehaviour
{
    public System.Action OnWin;
    
    //public MouseMovement playerPlunge;
    //public int plunges;
    //public int plungeGoal;
    public GameObject toiletPrefab;
    //public GameObject InvisibleWall;
    public GameObject winImg;
    public toiletOverlord[] toiletCount;
    public int uncloggedCount;
    public int uncloggedGoal;
    public Vector3[] positionsEasy;
    public Vector3[] positionsNormal;
    public Vector3[] positionsHard;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MinigameInit();
    }

    public void MinigameInit()  {
        uncloggedCount = 0;
        uncloggedGoal = 0;
        var oldToilets = GetComponentsInChildren<toiletOverlord>(true);
        foreach (var toilet in oldToilets)
        {
            if (toilet != null && toilet.gameObject != null)
                Destroy(toilet.gameObject);
        }
  
        switch (skillStatic.Skill)
        {
            case 0:
                foreach (Vector3 pos in positionsEasy)
                {
                    GameObject.Instantiate(toiletPrefab, transform.position + pos, transform.rotation, this.transform);
                }
                break;
            case 1:
                foreach (Vector3 pos in positionsNormal)
                {
                    GameObject.Instantiate(toiletPrefab, transform.position + pos, transform.rotation, this.transform);
                }
                break;
            case 2:
                foreach (Vector3 pos in positionsHard)
                {
                    GameObject.Instantiate(toiletPrefab, transform.position + pos, transform.rotation, this.transform);
                }
                break;
        }
toiletCount = GetComponentsInChildren<toiletOverlord>(true);
uncloggedGoal = toiletCount.Length - oldToilets.Length; // Atar con alambre
        //playerPlunge = FindFirstObjectByType<MouseMovement>();
        //plungeGoal = (Random.Range(4, 8));   
    }


    public void winCondition()
    {
        winImg.SetActive(true);
        StartCoroutine(WinDelayCoroutine());
    }

    private System.Collections.IEnumerator WinDelayCoroutine()
    {
        yield return new WaitForSeconds(2f);
		winImg.SetActive(false);
        if (OnWin != null) OnWin.Invoke();
    }
    public void SetSkill(int skillet)
    {
        skillStatic.Skill = skillet;
    }
}
