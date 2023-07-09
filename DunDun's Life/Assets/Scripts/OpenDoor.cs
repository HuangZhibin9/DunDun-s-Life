using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : ItemIteract
{
    bool IsFisrtTime = true;
    public override bool Grasp()
    {
        if (Input.GetKeyDown(KeyCode.E) && IsFisrtTime)
        {
            Debug.Log($"开门");
            IsFisrtTime = false;
        }

        return false;
    }
}
