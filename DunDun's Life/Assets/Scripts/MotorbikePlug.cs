using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorbikePlug : ItemIteract
{
    public GameObject MotorBike;
    public override bool Grasp()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            MotorBike.GetComponent<Motorbike>().power = false;
        }
        return false;
    }
}
