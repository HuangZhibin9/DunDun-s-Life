using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;

    private void Start()
    {
        agent.destination = target.position;
    }
}
