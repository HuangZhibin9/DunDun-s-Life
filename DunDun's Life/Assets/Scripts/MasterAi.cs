using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAi : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] Path;
    public bool isGo = false;
    public bool isBack = false;
    public GameObject stopcock;
    public int Index = 0;
    public float movementSpeed = 3f;
    Animator anim;
    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(Index);
        if (isGo)
        {
            if (Index >= Path.Length)
            {
                isGo = false;
                isBack = true;
                Index = 2;
                stopcock.GetComponent<Stopcock>().isSpring = false;
                anim.SetBool("isWalking", false);
            }
            Vector3 movement = new Vector3(Path[Index].transform.position.x - transform.position.x, 0, Path[Index].transform.position.z - transform.position.z);
            //Debug.Log(movement.magnitude);
            Vector3 direction = movement.normalized;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.15f);
                transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);
                anim.SetBool("isWalking", true);
            }

            if (movement.magnitude < 1f)
            {
                Index++;
            }
        }
        if (isBack)
        {
            if (Index < 1)
            {
                isBack = false;
                Index = 1;
                anim.SetBool("isWalking", false);
            }
            Vector3 movement = new Vector3(Path[Index].transform.position.x - transform.position.x, 0, Path[Index].transform.position.z - transform.position.z);

            Vector3 direction = movement.normalized;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.15f);
                transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);
                anim.SetBool("isWalking", true);
            }
            if (movement.magnitude < 1f)
            {
                Index--;
            }
        }
        if (!isGo && !isBack)
        {
            anim.SetBool("isWalking", false);
        }
    }
    public void StartGo()
    {
        if (!isGo)
        {
            isGo = true;
            isBack = false;
            Index += 1;
        }
    }
}
