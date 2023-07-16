using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kettle : MonoBehaviour
{
    public Vector3 oriPosition;
    public Vector3 oriRotation;
    public GameObject dog;
    public GameObject Master;
    public Transform Hand;
    public Transform HandUp;
    public Vector3 offsetPosition;
    public Vector3 offsetRotation;
    public Vector3 handPositon;
    ItemIteract item;

    bool isMoved = false;
    bool IsGrasping = false;

    public bool MasterCatch = false;

    private void Start()
    {
        item = this.GetComponent<ItemIteract>();
        oriPosition = GameObject.Find("KettlePosition").transform.position + new Vector3(10f, 0, 0);
    }

    private void Update()
    {
        if (MasterCatch)
        {
            Debug.Log($"Kettle should onhand");
            handPositon = Hand.position + offsetPosition;
            this.gameObject.transform.position = handPositon;
            this.gameObject.transform.rotation = Master.transform.rotation * Quaternion.Euler(offsetRotation);
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void PhoneReset()
    {
        MasterCatch = false;
        this.gameObject.transform.position = oriPosition;
        this.gameObject.transform.rotation = Quaternion.Euler(oriRotation);
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<BoxCollider>().enabled = true;
        this.GetComponent<ItemIteract>().enabled = true;
    }
}
