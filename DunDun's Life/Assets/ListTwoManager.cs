using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListTwoManager : MonoBehaviour
{
    public GameObject FinishOne;
    public GameObject FinishTwo;
    public GameObject FinishThree;
    public GameObject FinishFour;
    public void Finish(int x)
    {
        if (x == 1)
        {
            FinishOne.SetActive(true);
        }
        else if (x == 2)
        {
            FinishTwo.SetActive(true);
        }
        else if (x == 3)
        {
            FinishThree.SetActive(true);
        }
        else if (x == 4)
        {
            FinishFour.SetActive(true);
        }
    }

    public void Undo(int x)
    {
        if (x == 1)
        {
            FinishOne.SetActive(false);
        }
        else if (x == 2)
        {
            FinishTwo.SetActive(false);
        }
        else if (x == 3)
        {
            FinishThree.SetActive(false);
        }
        else if (x == 4)
        {
            FinishFour.SetActive(false);
        }
    }
}
