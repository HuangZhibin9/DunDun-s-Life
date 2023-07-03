using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paticle : MonoBehaviour
{
    public List<GameObject> ParticleAnims;
    private Dictionary<string, GameObject> ParticleAnimsDic;

    #region 单例
    public static Paticle Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            ParticleAnimsDic = new Dictionary<string, GameObject>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    private void Start()
    {
        foreach (var anim in ParticleAnims)
        {
            ParticleAnimsDic.Add(anim.name, anim);
        }
    }
    GameObject PlayParticle(string name, Vector3 position, Vector3 rotation)
    {
        if (!Instance.ParticleAnimsDic.ContainsKey(name))
        {
            Debug.LogWarning($"名为{name}的粒子效果不存在！");
            return null;
        }
        else
        {
            return Instantiate(ParticleAnimsDic[name], position, Quaternion.Euler(rotation));
        }
    }

}
