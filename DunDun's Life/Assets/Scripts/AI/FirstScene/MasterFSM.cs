using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum StateType
{
    Idle, CloseWater, CatchPhone,
}


[Serializable]
public class Parameter
{
    public Animator animator;
    public NavMeshAgent agent;
    public Transform target;
    public Vector3 oriPosition;

}

public class MasterFSM : MonoBehaviour
{
    private IState currentState;
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();
    public Parameter parameter;
    public GameObject emojiManager;


    void Start()
    {
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.CloseWater, new CloseWaterState(this));
        states.Add(StateType.CatchPhone, new CatchPhoneState(this));

        TransitionState(StateType.Idle);
    }

    void Update()
    {
        currentState.OnUpdate();

    }
    public void TransitionState(StateType type)
    {
        if (currentState != null)
            currentState.OnExit();
        currentState = states[type];
        currentState.OnEnter();
    }

}
