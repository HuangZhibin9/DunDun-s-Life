using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum ChildStateType
{
    ChildIdle, ChildShock
}


[Serializable]
public class ChildParameter
{
    public Animator animator;

}

public class ChildFSM : MonoBehaviour
{
    private IState currentState;
    private Dictionary<ChildStateType, IState> states = new Dictionary<ChildStateType, IState>();
    public ChildParameter parameter;
    public GameObject emojiManager;


    void Start()
    {
        states.Add(ChildStateType.ChildIdle, new ChildIdleState(this));
        states.Add(ChildStateType.ChildShock, new ChildShockState(this));
        TransitionState(ChildStateType.ChildIdle);
    }

    void Update()
    {
        currentState.OnUpdate();

    }
    public void TransitionState(ChildStateType type)
    {
        if (currentState != null)
            currentState.OnExit();
        currentState = states[type];
        currentState.OnEnter();
    }


}
