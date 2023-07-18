using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{
    public Material scarf;
    bool duck1 = false;
    bool duck2 = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "duck1")
        {
            duck1 = true;
        }
        if (other.name == "duck2")
        {
            duck2 = true;
        }

    }
    private void Update()
    {
        if (duck1 && duck2)
        {
            scarf.color = Color.yellow;
        }
    }

}
