using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Drive : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject DrivePosition;
    public Animator anim;
    bool isWalk = false;
    void Start()
    {
        agent.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (isWalk)
        {
            agent.destination = DrivePosition.transform.position;
            anim.SetBool("Walk", true);
        }
    }
    public void OpenDrive()
    {
        agent.enabled = true;
        isWalk = true;
        this.GetComponent<PlayerController>().enabled = false;
    }
    public void CloseDrive()
    {
        agent.enabled = false;
        isWalk = false;
        this.GetComponent<PlayerController>().enabled = true;
        anim.SetBool("Walk", false);
    }
}
