using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public void OnEnter()
    {

    }
    public void OnUpdate()
    {

    }
    public void OnExit()
    {

    }
    public ChildParameter GetParameters()
    {
        return default(ChildParameter);
    }
}

public class ChildIdleState : IState
{
    private ChildFSM manager;
    private ChildParameter parameter;
    private float timer;
    private GameObject dog;
    public ChildIdleState(ChildFSM manger)
    {
        this.manager = manger;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        //parameter.animator.SetBool("Walking", false);
        parameter.animator.SetBool("Shock", false);
        dog = GameObject.Find("dog");
    }
    public void OnUpdate()
    {
        Debug.Log(Vector3.Distance(manager.transform.position, dog.transform.position));
        if (Vector3.Distance(manager.transform.position, dog.transform.position) < 50f && Input.GetMouseButtonDown(0))
        {
            manager.TransitionState(ChildStateType.ChildShock);
        }
    }
    public void OnExit()
    {

    }
    public ChildParameter GetParameters()
    {
        return parameter;
    }
}

public class ChildShockState : IState
{
    private ChildFSM manager;
    private ChildParameter parameter;
    private float timer;
    static int NUM = 0;
    public ChildShockState(ChildFSM manger)
    {
        this.manager = manger;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.SetBool("Shock", true);
        if (NUM >= 2)
        {
            manager.emojiManager.GetComponent<MasterEmoji>().PlayEmoji("哭");
        }
        else
        {
            manager.emojiManager.GetComponent<MasterEmoji>().PlayEmoji("害怕");
        }
        manager.emojiManager.GetComponent<MasterEmoji>().image.enabled = true;
        timer = 0;
        NUM++;
    }
    public void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer > 2f)
        {
            manager.TransitionState(ChildStateType.ChildIdle);
        }
        if (NUM == 2)
        {
            GameObject.Find("IceCream").transform.SetParent(null);
            GameObject.Find("IceCream").GetComponent<Rigidbody>().useGravity = true;
            GameObject.Find("IceCream").GetComponent<BoxCollider>().enabled = true;
            GameObject.Find("IceCream").GetComponent<ItemIteract>().enabled = true;
        }
    }
    public void OnExit()
    {
        timer = 0;
        parameter.animator.SetBool("Shock", false);
        manager.emojiManager.GetComponent<MasterEmoji>().image.enabled = false;
    }
    public ChildParameter GetParameters()
    {
        return parameter;
    }
}
