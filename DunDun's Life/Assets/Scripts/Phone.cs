using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    public Vector3 oriPosition;
    public Vector3 oriRotation;
    public GameObject dog;
    public GameObject Master;
    public Transform Hand;
    public Vector3 offsetPosition;
    public Vector3 offsetRotation;
    public Vector3 handPositon;
    ItemIteract item;

    //bool isMoved = false;
    //bool IsGrasping = false;

    public bool MasterCatch = false;

    private void Start()
    {
        item = this.GetComponent<ItemIteract>();
        oriPosition = this.transform.position;
    }

    public void CallMaster()
    {
        //isMoved = true;
        //IsGrasping = true;
        if (Master.GetComponent<MasterFSM>().currentState == Master.GetComponent<MasterFSM>().states[StateType.Idle])
        {
            Master.GetComponent<MasterFSM>().TransitionState(StateType.CatchPhone);
            Master.GetComponent<MasterFSM>().parameter.target = this.transform;
            Master.GetComponent<MasterFSM>().parameter.oriPosition = this.transform.position;
        }

    }
    private void Update()
    {
        if (MasterCatch)
        {
            Debug.Log($"Phone should onhand");
            handPositon = Hand.position + offsetPosition;
            this.gameObject.transform.position = handPositon;
            this.gameObject.transform.rotation = Master.transform.rotation * Quaternion.Euler(offsetRotation);
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.GetComponent<BoxCollider>().enabled = false;
        }
        if (this.GetComponent<ItemIteract>().IsGrasping)
        {
            GameObject.Find("ListManager").GetComponent<ListOneManager>().Finish(2);
        }
    }

    public void PhoneReset()
    {
        MasterCatch = false;
        this.gameObject.transform.position = oriPosition + offsetPosition;
        this.gameObject.transform.rotation = Quaternion.Euler(oriRotation);
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<BoxCollider>().enabled = true;
        this.GetComponent<ItemIteract>().enabled = true;

    }
}
