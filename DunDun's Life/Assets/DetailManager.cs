using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailManager : MonoBehaviour
{
    public float timer = 0f;
    public GameObject ListTile;
    public GameObject Flag;
    public bool isUsing = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5f)
        {
            ListTile.SetActive(false);
            //Flag.SetActive(true);
        }
        if (isUsing)
        {
            timer = 0f;
        }
    }

    public void ShowListTile()
    {
        ListTile.SetActive(!ListTile.activeSelf);
        timer = 0f;
    }


}
