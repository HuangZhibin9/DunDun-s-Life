using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{

    float movementSpeed = 3;
    public float walkSpeed = 8;
    public float runSpeed = 14;
    public float jumpForce = 300;
    public float timeBeforeNextJump = 1.2f;
    private float canJump = 0f;
    Animator anim;
    Rigidbody rb;
    [SerializeField]
    private CinemachineVirtualCamera virtualRunCamera = null;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        virtualRunCamera.enabled = false;
    }

    void Update()
    {
        ControllPlayer();
        DogSound();
    }

    void DogSound()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioManger.PlayAudio("DogSoundOne");
        }
        if (Input.GetMouseButtonDown(1))
        {
            AudioManger.PlayAudio("DogSoundTwo");
        }
    }
    void ControllPlayer()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            anim.SetInteger("Walk", 1);
        }
        else
        {
            anim.SetInteger("Walk", 0);
        }

        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);

        if (Input.GetButtonDown("Jump") && Time.time > canJump)
        {
            rb.AddForce(0, jumpForce, 0);
            canJump = Time.time + timeBeforeNextJump;
            anim.SetTrigger("jump");
        }

        if (Input.GetKeyDown("left shift"))
        {
            movementSpeed = runSpeed;
            anim.speed = 2f;
            virtualRunCamera.enabled = true;
        }
        if (Input.GetKeyUp("left shift"))
        {
            movementSpeed = walkSpeed;
            anim.speed = 1f;
            virtualRunCamera.enabled = false;
        }
    }
}