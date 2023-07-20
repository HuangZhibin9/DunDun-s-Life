using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterEmoji : MonoBehaviour
{
    public float rotationSpeed = 100f;

    public Transform cameraTransform;
    public Image image;

    public List<Sprite> Images;
    public Dictionary<string, Sprite> ImagesDic;
    public Transform target;
    public Vector3 Offset;


    private void Awake()
    {
        ImagesDic = new Dictionary<string, Sprite>();
    }
    private void Start()
    {
        foreach (var image in Images)
        {
            ImagesDic.Add(image.name, image);
            Debug.Log(image.name);
        }
    }

    void Update()
    {
        UpdateRotation();
        UpdatePosition();
    }

    void UpdateRotation()
    {
        transform.LookAt(cameraTransform);
    }
    void UpdatePosition()
    {
        transform.position = target.position + Offset;
    }

    public void PlayEmoji(string name)
    {

        if (!ImagesDic.ContainsKey(name))
        {
            Debug.LogWarning($"名为{name}的Emoji不存在！");
        }
        else
        {
            image.sprite = ImagesDic[name];
        }
    }
    public void reset()
    {
        image.enabled = false;
    }
}
