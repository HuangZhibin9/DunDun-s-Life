using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{

    float movementSpeed;        //当前速度
    public float walkSpeed;     //行走速度预设
    public float runSpeed = 14; //奔跑速度预设
    public float jumpForce = 300;
    public float timeBeforeNextJump = 1.2f;
    private float canJump = 0f;
    private float timer = 0f;
    Animator anim;
    Rigidbody rb;
    [SerializeField]

    public List<GameObject> Items;      //附近可交互的物品
    public GameObject TheClosestItem;   //最近的物品
    public float Distance; //与最近物品的距离
    public CinemachineVirtualCamera virtualRunCamera = null;    //奔跑时的虚拟摄像机

    //当有物品进入Trigger时，将其添加至 Items 列表
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Items")
        {
            Items.Add(other.gameObject);
        }
    }

    //当有物品离开Trigger时，将其移除 Items 列表
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Items")
        {
            Items.Remove(other.gameObject);
            other.GetComponent<Outline>().enabled = false;
        }

    }
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        virtualRunCamera.enabled = false;
    }

    void FixedUpdate()
    {
        //狗的行动控制
        ControllPlayer();
        //得到最近的物品

    }
    private void Update()
    {
        timer += Time.deltaTime;
        DogSound();
        Distance = GetMinDistanceItem();

        //如果最近的物品不为空，则执行该物品的 Grasp()
        if (TheClosestItem != null)
        {
            //Grasp中检测是否按E，从而抓取物品和放下物品
            //如果距离小于 X ，则执行Grasp()
            if (Distance < 35f)
            {
                bool flag = TheClosestItem.GetComponent<ItemIteract>().Grasp();

                if (flag)
                {
                    RemoveItem(TheClosestItem.name);
                }
            }

        }
    }
    public void RemoveItem(string name)
    {
        for (int i = 0; i < Items.Count;)
        {
            if (Items[i].name == name)
            {
                Items.Remove(Items[i]);
            }
            else i++;
        }
    }
    //左键狗叫 右键卖萌
    void DogSound()
    {

        if (Input.GetMouseButtonDown(0))
        {
            //AudioManger.PlayAudio("DogSoundOne");
        }
        if (Input.GetMouseButtonDown(1))
        {
            timer = 0;
            //AudioManger.PlayAudio("DogSoundTwo");
            anim.SetBool("Cute", true);

            Debug.Log("Cute");
        }
        if (timer > 1.5f && anim.GetBool("Cute"))
        {
            anim.SetBool("Cute", false);
        }
    }

    //得到最近的物品
    float GetMinDistanceItem()
    {
        float minDistance = 100000f;
        for (int i = 0; i < Items.Count; ++i)
        {
            float distance = Vector3.Distance(Items[i].transform.position, transform.GetChild(0).position);
            if (distance < minDistance)
            {
                TheClosestItem = Items[i];
                minDistance = distance;
                TheClosestItem.GetComponent<Outline>().enabled = true;
            }
            else
            {
                Items[i].GetComponent<Outline>().enabled = false;
            }
        }
        return minDistance;
    }

    //狗的移动控制
    void ControllPlayer()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
            //anim.SetInteger("Walk", 0);
        }

        //射线检测，如果为 Barrier 则不能继续前进
        RaycastHit hit;
        if (Physics.Raycast(transform.position, movement, out hit) && hit.collider.tag == "Barrier")
        {
            //Debug.Log($"hit.tag = {hit.collider.tag}");

            //Debug.DrawLine(transform.position, hit.point, new Color(0, 1, 1), 200f);
            float hitLen = (transform.position - hit.point).magnitude;
            //Debug.Log($"hit distance = {hitLen}");
            if (hitLen < 5f)
            {
                //don't move
            }
            else
            {
                transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);
            }
        }
        else
        {
            transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);
        }

        //rb.velocity = movement * movementSpeed;

        if (Input.GetButtonDown("Jump") && Time.time > canJump)
        {
            rb.AddForce(0, jumpForce, 0);
            canJump = Time.time + timeBeforeNextJump;
            anim.SetTrigger("jump");
        }

        if (Input.GetKey("left shift"))
        {
            movementSpeed = runSpeed;
            virtualRunCamera.enabled = true;
        }
        else
        {
            movementSpeed = walkSpeed;
            virtualRunCamera.enabled = false;
        }

    }
}