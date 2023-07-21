using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDetail : MonoBehaviour
{
    public GameObject target;
    public void OpenOrClose()
    {
        target.SetActive(!target.activeSelf);
    }
}
