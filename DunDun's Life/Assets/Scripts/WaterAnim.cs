using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAnim : MonoBehaviour
{
    public GameObject waterAnim;
    public Transform BornPoint;
    public Vector3 OffsetPosition;
    public Vector3 OffsetRotation;
    private GameObject obj = null;
    bool isSoundOn = false;
    private void FixedUpdate()
    {
        if (obj == null && GetComponent<ItemIteract>().IsGrasping)
        {
            obj = Instantiate(waterAnim, BornPoint.position + OffsetPosition, BornPoint.rotation * Quaternion.Euler(OffsetRotation));
            obj.transform.SetParent(this.transform);
            if (!isSoundOn)
            {
                AudioManger.PlayAudio("洒水声");
                isSoundOn = true;
            }
        }
        if (obj != null && !GetComponent<ItemIteract>().IsGrasping)
        {
            AudioManger.StopAudio("洒水声");
            isSoundOn = false;
            Destroy(obj);

        }
    }

}
