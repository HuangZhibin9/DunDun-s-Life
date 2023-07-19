using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIteract : MonoBehaviour
{
    //狗的Transform组件
    public Transform Dog;
    public PlayerController DogController;
    //位置偏移
    public Vector3 OffsetPosition;
    //旋转偏移
    public Vector3 OffsetRotation;
    //是否抓取
    public bool IsGrasping = false;
    public bool ForGrasp = true;

    bool flag = false;


    // Update is called once per frame
    void Update()
    {
        if (IsGrasping)
        {
            transform.eulerAngles = Dog.eulerAngles + OffsetRotation;
            transform.position = Dog.position + OffsetPosition;
        }
    }

    public virtual bool Grasp()
    {
        flag = false;
        if (ForGrasp)
        {
            //因为在放下物品时，会开启Collider组件，会导致PlayerController脚本中的物品List增加一个元素，需要把这个删掉
            if (IsGrasping == false)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    IsGrasping = true;
                    this.GetComponent<Collider>().enabled = false;
                    transform.eulerAngles = Dog.eulerAngles + OffsetRotation;
                    transform.position = Dog.position + OffsetPosition;
                    if (this.name == "Phone")
                    {
                        this.GetComponent<Phone>().CallMaster();
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PutDown();
                    flag = true;
                }
            }
        }

        return flag;
    }

    public void PutDown()
    {
        IsGrasping = false;
        this.GetComponent<Collider>().enabled = true;
    }
}
