using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorials : MonoBehaviour
{
    //狗的位置信息
    public Transform dogTrans;
    //偏移量
    public Vector3 OffSet;
    //字体的透明度
    public float FaceDilate = 0;
    //目前处在引导的阶段数
    public int GuidenceIndex = 1;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = dogTrans.position + OffSet;
        Guidence();
    }

    void Guidence()
    {
        if (GuidenceIndex == 1)
        {
            if (FaceDilate > -1 && GuidenceIndex == 1
                && Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A)
                || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine("TextFade");
                GuidenceIndex = 2;
            }
        }
        else if (GuidenceIndex == 2)
        {
            if (FaceDilate < -0.9f && GuidenceIndex == 2)
            {
                GetComponent<TextMeshPro>().text = "LeftShift Run";
                StartCoroutine("TextPresent");
            }
            if (FaceDilate > -0.1f && GuidenceIndex == 2 && GetComponent<TextMeshPro>().text == "LeftShift Run")
            {
                StartCoroutine("TextFade");
                GuidenceIndex = 3;
            }
        }
        else if (GuidenceIndex == 3)
        {
            if (FaceDilate > -1 && GuidenceIndex == 3
                && Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine("TextFade");
                GuidenceIndex = 4;
            }
        }
        else if (GuidenceIndex == 4)
        {
            if (FaceDilate > -1 && GuidenceIndex == 4
                && Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine("TextFade");
                GuidenceIndex = 5;
            }
        }
        else if (GuidenceIndex == 5)
        {
            if (FaceDilate > -1f && GuidenceIndex == 5)
            {
                StartCoroutine("TextFade");
                GuidenceIndex = 5;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Items" && GuidenceIndex == 4)
        {
            GetComponent<TextMeshPro>().text = "E to catch/throw";
            StartCoroutine("TextPresent");
        }
    }

    //字体消失动画
    IEnumerator TextFade()
    {
        while (FaceDilate > -1f)
        {
            FaceDilate -= 0.02f;
            GetComponent<MeshRenderer>().material.SetFloat("_FaceDilate", FaceDilate);
            yield return new WaitForSeconds(0.02f);
        }
        yield break;
    }
    //字体出现动画
    IEnumerator TextPresent()
    {
        while (FaceDilate < 0f)
        {
            FaceDilate += 0.02f;
            GetComponent<MeshRenderer>().material.SetFloat("_FaceDilate", FaceDilate);
            yield return new WaitForSeconds(0.02f);
        }
        yield break;
    }
}
