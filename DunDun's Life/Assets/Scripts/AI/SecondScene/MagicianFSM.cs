using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum MagicianStateType
{
    MagicianIdle, MagicianShock, MagicianCatchQiqiu, MagicianDrive,
}


[Serializable]
public class MagicianParameter
{
    public Animator animator;
    public NavMeshAgent agent;
    public Transform target;
    public Vector3 oriPosition;

}

public class MagicianFSM : MonoBehaviour
{
    private IState currentState;
    private Dictionary<MagicianStateType, IState> states = new Dictionary<MagicianStateType, IState>();
    public MagicianParameter parameter;
    public GameObject emojiManager;
    public int num = 0;
    public bool cheer = true;


    void Start()
    {
        states.Add(MagicianStateType.MagicianIdle, new MagicianIdleState(this));
        states.Add(MagicianStateType.MagicianShock, new MagicianShockState(this));
        states.Add(MagicianStateType.MagicianCatchQiqiu, new MagicianCatchQiqiuState(this));
        states.Add(MagicianStateType.MagicianDrive, new MagicianDriveState(this));
        TransitionState(MagicianStateType.MagicianIdle);
    }

    void Update()
    {
        currentState.OnUpdate();

    }
    public void TransitionState(MagicianStateType type)
    {
        if (currentState != null)
            currentState.OnExit();
        currentState = states[type];
        currentState.OnEnter();
    }


}
