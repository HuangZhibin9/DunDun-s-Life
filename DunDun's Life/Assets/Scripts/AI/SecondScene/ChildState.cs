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
        timer = 0f;
        manager.emojiManager.GetComponent<MasterEmoji>().PlayEmoji("小黄鸭");
    }
    public void OnUpdate()
    {
        //Debug.Log(Vector3.Distance(manager.transform.position, dog.transform.position));
        timer += Time.deltaTime;
        if (timer < 0)
        {
            manager.emojiManager.GetComponent<MasterEmoji>().image.enabled = true;
        }
        if (timer > 0)
        {
            manager.emojiManager.GetComponent<MasterEmoji>().image.enabled = false;
        }
        if (timer > 3f)
        {
            timer -= 6f;
        }
        if (Vector3.Distance(manager.transform.position, dog.transform.position) < 50f && Input.GetMouseButtonDown(0))
        {
            manager.TransitionState(ChildStateType.ChildShock);
        }
        if (Vector3.Distance(manager.transform.position, dog.transform.position) < 80f && Input.GetMouseButtonDown(1))
        {
            manager.TransitionState(ChildStateType.ChildEnjoy);
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

public class ChildEnjoyState : IState
{
    private ChildFSM manager;
    private ChildParameter parameter;
    private float timer;
    private GameObject dog;
    private GameObject duck1;
    private GameObject duck2;
    private GameObject duck3;
    private GameObject duck = null;

    public ChildEnjoyState(ChildFSM manger)
    {
        this.manager = manger;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        manager.emojiManager.GetComponent<MasterEmoji>().PlayEmoji("爱心");
        manager.emojiManager.GetComponent<MasterEmoji>().image.enabled = true;
        timer = 0f;
        if (manager.IceFinish == false)
        {
            duck1 = GameObject.Find("duck1");
            duck2 = GameObject.Find("duck2");
            duck3 = GameObject.Find("duck3");
            if (Vector3.Distance(manager.transform.position, duck1.transform.position) < 50f)
            {
                duck = duck1;
            }
            else if (Vector3.Distance(manager.transform.position, duck2.transform.position) < 50f)
            {
                duck = duck2;
            }
            else if (Vector3.Distance(manager.transform.position, duck3.transform.position) < 50f)
            {
                duck = duck3;
            }
            else
            {
                duck = null;
            }
        }
    }
    public void OnUpdate()
    {
        timer += Time.deltaTime;
        manager.transform.LookAt(GameObject.Find("dog").transform);

        if (timer > 2f)
        {
            manager.TransitionState(ChildStateType.ChildIdle);
        }
        if (duck != null)
        {
            GameObject.Find("IceCream").transform.SetParent(null);
            GameObject.Find("IceCream").GetComponent<Rigidbody>().useGravity = true;
            GameObject.Find("IceCream").GetComponent<BoxCollider>().enabled = true;
            GameObject.Find("IceCream").GetComponent<ItemIteract>().enabled = true;
            parameter.animator.SetBool("Ice", false);
            duck.SetActive(false);
            manager.IceFinish = true;
            GameObject.Find("ListManager").GetComponent<ListTwoManager>().Finish(2);
        }
    }
    public void OnExit()
    {
        manager.emojiManager.GetComponent<MasterEmoji>().image.enabled = false;
    }
    public ChildParameter GetParameters()
    {
        return default(ChildParameter);
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
            parameter.animator.SetBool("Ice", false);
            GameObject.Find("ListManager").GetComponent<ListTwoManager>().Finish(2);
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
