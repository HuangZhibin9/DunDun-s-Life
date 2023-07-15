
using UnityEngine;
using UnityEngine.AI;

public class IdleState : IState
{
    private MasterFSM manager;
    private Parameter parameter;
    private float timer;

    public IdleState(MasterFSM manger)
    {
        this.manager = manger;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        //parameter.animator.Play("Idle");
        parameter.animator.SetBool("Walking", false);
        parameter.animator.SetBool("CloseWater", false);
        parameter.animator.SetBool("Shock", false);
    }
    public void OnUpdate()
    {

    }
    public void OnExit()
    {
        timer = 0;
    }
}



public class CloseWaterState : IState
{
    private MasterFSM manager;
    private Parameter parameter;
    private float timer;

    public CloseWaterState(MasterFSM manger)
    {
        this.manager = manger;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.SetBool("Walking", true);
        parameter.agent.destination = parameter.target.position;
        timer = 0;
    }
    public void OnUpdate()
    {
        //parameter.agent.destination = parameter.target.position;
        if (timer == 0 && Vector3.Distance(manager.gameObject.transform.position, parameter.target.position) < 32f)
        {
            Debug.Log($"CloseWater");
            parameter.animator.SetBool("Walking", false);
            parameter.animator.SetBool("CloseWater", true);
            timer += Time.deltaTime;
        }
        if (timer > 0)
        {
            timer += Time.deltaTime;
        }
        if (timer > 5f)
        {
            Debug.Log($"Return");
            parameter.agent.destination = parameter.oriPosition;
            parameter.animator.SetBool("Walking", true);
            parameter.animator.SetBool("CloseWater", false);
        }
        if (timer > 0 && Vector3.Distance(manager.gameObject.transform.position, parameter.oriPosition) < 32f)
        {
            Debug.Log($"Arrived");
            manager.TransitionState(StateType.Idle);
        }

    }
    public void OnExit()
    {
        timer = 0;
    }
}

public class CatchPhoneState : IState
{
    private MasterFSM manager;
    private Parameter parameter;
    private float timer;
    int Index = 0;

    public CatchPhoneState(MasterFSM manger)
    {
        this.manager = manger;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.SetBool("Shock", true);
        timer = 0;
        manager.emojiManager.GetComponent<MasterEmoji>().PlayEmoji("Angry");
        manager.emojiManager.GetComponent<MasterEmoji>().image.enabled = true;
    }
    public void OnUpdate()
    {

        timer += Time.deltaTime;

        if (timer > 2f && Index == 0)
        {
            parameter.animator.SetBool("Shock", false);
            parameter.agent.destination = parameter.target.position;
            parameter.animator.SetBool("Walking", true);
            manager.emojiManager.GetComponent<MasterEmoji>().PlayEmoji("生气");
        }

        if (Vector3.Distance(manager.gameObject.transform.position, parameter.target.position) < 32f && Index == 0)
        {
            parameter.target.GetComponent<ItemIteract>().PutDown();
            Index = 2;
        }

        if (Vector3.Distance(manager.gameObject.transform.position, parameter.target.position) < 32f && Index == 2)
        {
            parameter.animator.SetBool("Walking", false);
            parameter.animator.SetBool("CloseWater", true);
            timer = 0;
            Index = 3;
            parameter.target.GetComponent<ItemIteract>().enabled = false;
        }
        if (timer > 2f && Index == 3)
        {
            parameter.animator.SetBool("CloseWater", false);
            parameter.animator.SetBool("Walking", true);
            parameter.agent.destination = parameter.oriPosition;
            Index = 4;
            manager.emojiManager.GetComponent<MasterEmoji>().reset();
        }

        if (Index == 4 || Index == 5)
        {
            parameter.target.GetComponent<Phone>().MasterCatch = true;
        }
        if (Vector3.Distance(manager.gameObject.transform.position, parameter.oriPosition) < 32f && Index == 4)
        {
            parameter.animator.SetBool("Walking", false);
            // parameter.animator.SetBool("CloseWater", true);
            timer = 0;
            Index = 5;
        }
        if (timer > 2f && Index == 5)
        {
            //parameter.target.GetComponent<Phone>().MasterCatch = false;
            parameter.target.GetComponent<Phone>().PhoneReset();
            timer = 0;
            Index = 6;
        }
        if (timer > 2f && Index == 6)
        {
            parameter.animator.SetBool("Walking", true);
            parameter.oriPosition = parameter.target.transform.position + new Vector3(100, 0, 0);
            parameter.agent.destination = parameter.target.transform.position + new Vector3(100, 0, 0);
        }
        if (timer > 4f && Index == 6)
        {
            manager.TransitionState(StateType.Idle);
        }
    }
    public void OnExit()
    {
        timer = 0;
        Index = 0;
    }
}
