using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianState : IState
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
    public MagicianParameter GetParameters()
    {
        return default(MagicianParameter);
    }
}

public class MagicianIdleState : IState
{

    private MagicianFSM manager;
    private MagicianParameter parameter;
    private float timer;
    private GameObject dog;
    private GameObject qiqiu;
    public MagicianIdleState(MagicianFSM manger)
    {
        this.manager = manger;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        dog = GameObject.Find("dog");
        qiqiu = GameObject.Find("Item1");
        if (manager.num > 1)
        {
            manager.cheer = false;
        }
        parameter.animator.SetBool("Cheer", manager.cheer);
        parameter.animator.SetBool("Walk", false);
        parameter.animator.SetBool("Catch", false);
        parameter.animator.SetBool("Shock", false);
        parameter.animator.SetBool("Drive", false);
    }
    public void OnUpdate()
    {
        //Debug.Log(Vector3.Distance(manager.transform.position, dog.transform.position));
        if (manager.num > 1)
        {
            if (Vector3.Distance(manager.transform.position, dog.transform.position) < 50f)
            {
                manager.TransitionState(MagicianStateType.MagicianDrive);
            }
        }
        else
        {
            if (Vector3.Distance(manager.transform.position, dog.transform.position) < 80f && Input.GetMouseButtonDown(0))
            {
                manager.TransitionState(MagicianStateType.MagicianShock);
            }
            if (qiqiu.GetComponent<ItemIteract>().IsGrasping == true)
            {
                manager.TransitionState(MagicianStateType.MagicianCatchQiqiu);
            }
        }
    }
    public void OnExit()
    {

    }
    public MagicianParameter GetParameters()
    {
        return default(MagicianParameter);
    }
}

public class MagicianDriveState : IState
{
    private MagicianFSM manager;
    private MagicianParameter parameter;
    private float timer;
    private GameObject dog;
    public MagicianDriveState(MagicianFSM manger)
    {
        this.manager = manger;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        dog = GameObject.Find("dog");
        manager.transform.LookAt(dog.transform);
        parameter.animator.SetBool("Drive", true);
        manager.emojiManager.GetComponent<MasterEmoji>().PlayEmoji("生气");
        manager.emojiManager.GetComponent<MasterEmoji>().image.enabled = true;
        timer = 0f;
        dog.GetComponent<Drive>().OpenDrive();
    }
    public void OnUpdate()
    {
        timer += Time.deltaTime;
        manager.transform.LookAt(GameObject.Find("dog").transform);
        if (timer > 2f)
        {
            manager.TransitionState(MagicianStateType.MagicianIdle);
        }
    }
    public void OnExit()
    {
        timer = 0f;
        parameter.animator.SetBool("Drive", false);
        manager.emojiManager.GetComponent<MasterEmoji>().image.enabled = false;
        dog.GetComponent<Drive>().CloseDrive();
    }
    public MagicianParameter GetParameters()
    {
        return default(MagicianParameter);
    }
}

public class MagicianShockState : IState
{
    private MagicianFSM manager;
    private MagicianParameter parameter;
    private float timer;
    static int NUM = 0;
    public MagicianShockState(MagicianFSM manger)
    {
        this.manager = manger;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        manager.num++;
        parameter.animator.SetBool("Cheer", false);
        parameter.animator.SetBool("Shock", true);
        if (NUM >= 1)
        {
            manager.emojiManager.GetComponent<MasterEmoji>().PlayEmoji("生气");
        }
        else
        {
            manager.emojiManager.GetComponent<MasterEmoji>().PlayEmoji("Angry");
        }
        manager.emojiManager.GetComponent<MasterEmoji>().image.enabled = true;
        timer = 0;
        NUM++;
    }
    public void OnUpdate()
    {
        timer += Time.deltaTime;
        manager.transform.LookAt(GameObject.Find("dog").transform);
        if (timer > 2f)
        {
            manager.TransitionState(MagicianStateType.MagicianIdle);
        }
    }
    public void OnExit()
    {
        timer = 0;
        parameter.animator.SetBool("Shock", false);
        manager.emojiManager.GetComponent<MasterEmoji>().image.enabled = false;
    }
    public MagicianParameter GetParameters()
    {
        return parameter;
    }
}

public class MagicianCatchQiqiuState : IState
{
    private MagicianFSM manager;
    private MagicianParameter parameter;
    private float timer;
    int Index = 0;

    public MagicianCatchQiqiuState(MagicianFSM manger)
    {
        this.manager = manger;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        manager.num++;
        parameter.animator.SetBool("Cheer", false);
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
            parameter.animator.SetBool("Walk", true);
            manager.emojiManager.GetComponent<MasterEmoji>().PlayEmoji("Angry");
        }

        if (Vector3.Distance(manager.gameObject.transform.position, parameter.target.position) < 32f && Index == 0)
        {
            parameter.target.GetComponent<ItemIteract>().PutDown();
            GameObject.Find("dog").GetComponent<PlayerController>().RemoveItem("Item1");
            Index = 2;
        }

        if (Vector3.Distance(manager.gameObject.transform.position, parameter.target.position) < 32f && Index == 2)
        {
            parameter.animator.SetBool("Walk", false);
            parameter.animator.SetBool("Catch", true);
            timer = 0;
            Index = 3;
            parameter.target.GetComponent<ItemIteract>().enabled = false;
        }
        if (timer > 2f && Index == 3)
        {
            parameter.animator.SetBool("Catch", false);
            parameter.animator.SetBool("Walk", true);
            parameter.agent.destination = GameObject.Find("PutDownQiqiu").transform.position;
            Index = 4;
            manager.emojiManager.GetComponent<MasterEmoji>().reset();
        }

        if (Index == 4 || Index == 5)
        {
            parameter.target.GetComponent<Qiqiu>().MagicianCatch = true;
        }
        if (Vector3.Distance(manager.gameObject.transform.position, GameObject.Find("PutDownQiqiu").transform.position) < 32f && Index == 4)
        {
            parameter.animator.SetBool("Walk", false);
            parameter.animator.SetBool("PutDown", true);
            timer = 0;
            Index = 5;
        }
        if (timer > 1f && Index == 5)
        {
            //parameter.target.GetComponent<Phone>().MasterCatch = false;
            parameter.target.GetComponent<Qiqiu>().QiqiuReset();
            timer = 0;
            Index = 6;
        }
        if (timer > 1f && Index == 6)
        {
            parameter.animator.SetBool("PutDown", false);
            parameter.animator.SetBool("Walk", true);
            parameter.agent.destination = parameter.oriPosition;
        }
        if (timer > 4f && Index == 6)
        {

            manager.TransitionState(MagicianStateType.MagicianIdle);
        }

    }
    public void OnExit()
    {
        timer = 0;
        Index = 0;
    }

    public MagicianParameter GetParameters()
    {
        return parameter;
    }
}
