using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenDoor : ItemIteract
{
    int Index = 0;
    public GameObject text;
    public GameObject GoOutTimeline;
    bool canOpen = false;
    public GameObject NewMaster;
    public GameObject OldMaster;
    public override bool Grasp()
    {


        return false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Index == 0 && canOpen)
        {
            OldMaster.SetActive(false);
            NewMaster.SetActive(true);
            GoOutTimeline.SetActive(true);
            this.transform.position = new Vector3(0, 0, 0);
            text.SetActive(false);
            StartCoroutine("NextScene");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "dog")
        {
            text.SetActive(true);
            canOpen = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "dog")
        {
            text.SetActive(false);
            canOpen = false;
        }
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene(2);
        yield break;
    }
}
