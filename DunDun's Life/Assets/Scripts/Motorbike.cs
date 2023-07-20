using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motorbike : MonoBehaviour
{
    public GameObject BikeCanvas;
    public GameObject dog;
    public float timer = 0f;
    public bool isShowing = false;
    public bool can = true;
    public bool power = true;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5f && isShowing)
        {
            BikeCanvas.SetActive(false);
            isShowing = false;
            dog.GetComponent<PlayerController>().walkSpeed += 40;
            dog.GetComponent<PlayerController>().runSpeed += 40;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "dog" && can && power)
        {
            BikeCanvas.SetActive(true);
            isShowing = true;
            dog.GetComponent<PlayerController>().walkSpeed -= 40;
            dog.GetComponent<PlayerController>().runSpeed -= 40;
            can = false;
            StartCoroutine("ResetSound");
            timer = 0f;
        }
    }

    IEnumerator ResetSound()
    {
        yield return new WaitForSeconds(30f);
        can = true;
    }
}
