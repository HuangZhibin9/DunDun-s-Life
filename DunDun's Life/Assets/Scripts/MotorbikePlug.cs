using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorbikePlug : ItemIteract
{
    public GameObject MotorBike;
    public GameObject Dianxian;
    public GameObject Dianxian2;
    public override bool Grasp()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            MotorBike.GetComponent<Motorbike>().power = false;
            Dianxian.SetActive(false);
            Dianxian2.SetActive(true);
        }
        return false;
    }
}
