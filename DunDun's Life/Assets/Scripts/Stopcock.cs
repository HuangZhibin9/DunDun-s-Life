using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stopcock : ItemIteract
{
    public override bool Grasp()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"喷水");
        }

        return false;
    }
}
