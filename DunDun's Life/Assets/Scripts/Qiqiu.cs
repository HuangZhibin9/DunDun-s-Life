using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qiqiu : MonoBehaviour
{
    public Vector3 oriPosition;
    public Vector3 oriRotation;
    public GameObject dog;
    public GameObject Magician;
    public Transform Hand;
    public Vector3 offsetPosition;
    public Vector3 offsetRotation;
    public Vector3 handPositon;
    ItemIteract item;

    //bool isMoved = false;
    //bool IsGrasping = false;

    public bool MagicianCatch = false;

    private void Start()
    {
        item = this.GetComponent<ItemIteract>();
        oriPosition = this.transform.position;
        oriRotation = this.transform.rotation.eulerAngles;
    }

    public void CallMagician()
    {
        //isMoved = true;
        //IsGrasping = true;
        Magician.GetComponent<MagicianFSM>().TransitionState(MagicianStateType.MagicianCatchQiqiu);
    }
    private void Update()
    {
        if (MagicianCatch)
        {
            Debug.Log($"Phone should onhand");
            handPositon = Hand.position + offsetPosition;
            this.gameObject.transform.position = handPositon;
            this.gameObject.transform.rotation = Hand.transform.rotation * Quaternion.Euler(offsetRotation);
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void QiqiuReset()
    {
        MagicianCatch = false;
        this.gameObject.transform.position = oriPosition + offsetPosition;
        this.gameObject.transform.rotation = Quaternion.Euler(oriRotation);
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<BoxCollider>().enabled = true;
        this.GetComponent<ItemIteract>().enabled = true;

    }
}
