using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIteract : MonoBehaviour
{
    public Transform colliderItem;
    public Vector3 OffsetPosition;
    public Vector3 OffsetRotation;
    public bool IsClosed = false;
    public bool IsGrasping = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            IsClosed = true;
        }
        //else IsClosed = false;
    }
    private void OnTriggerExit(Collider other)
    {
        IsClosed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsClosed)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                IsGrasping = true;
                this.GetComponent<Collider>().enabled = false;
                //transform.SetParent(colliderItem.transform);
                transform.eulerAngles = colliderItem.eulerAngles + OffsetRotation;
                transform.position = colliderItem.position + OffsetPosition;
                IsClosed = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {

                IsGrasping = false;
                this.GetComponent<Collider>().enabled = true;
            }
        }

        if (IsGrasping)
        {
            transform.eulerAngles = colliderItem.eulerAngles + OffsetRotation;
            transform.position = colliderItem.position + OffsetPosition;
        }
    }
}
