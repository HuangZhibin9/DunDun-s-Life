using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stopcock : ItemIteract
{
    public bool isSpring = false;
    public GameObject Master;
    public override bool Grasp()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isSpring = true;
            Master.GetComponent<MasterFSM>().parameter.target = this.transform;
            Master.GetComponent<MasterFSM>().TransitionState(StateType.CloseWater);
        }

        return false;
    }
    private void Update()
    {
        if (isSpring)
        {
            Debug.Log($"Spring");
        }
    }
}
