using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum StateType
{
    Idle, CloseWater, CatchPhone, Wetted,
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
    public GameObject Kettle;
    public bool isWetted = false;


    void Start()
    {
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.CloseWater, new CloseWaterState(this));
        states.Add(StateType.CatchPhone, new CatchPhoneState(this));
        states.Add(StateType.Wetted, new WettedState(this));

        TransitionState(StateType.Idle);
    }

    void Update()
    {
        currentState.OnUpdate();
        if (Vector3.Distance(this.transform.position, GameObject.Find("Kettle").transform.position) < 25f && GameObject.Find("Kettle").GetComponent<ItemIteract>().IsGrasping == true && isWetted == false)
        {
            Debug.Log("Call the master");
            isWetted = true;
        }
    }
    public void TransitionState(StateType type)
    {
        if (currentState != null)
            currentState.OnExit();
        currentState = states[type];
        currentState.OnEnter();
    }


}
