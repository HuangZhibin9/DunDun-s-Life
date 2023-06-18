using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


/// <summary>
/// 音频管理器，存储所有音频并且控制播放和停止
/// <summary>
public class AudioManger : MonoBehaviour
{

    [System.Serializable]
    public class Sound
    {
        [Header("音频剪辑")]
        public AudioClip clip;

        [Header("音频分组")]
        public AudioMixerGroup outputGroup;

        [Header("音频音量")]
        public float volume = 1;

        [Header("音频是否开局播放")]
        public bool playOnAwake = false; ///

        [Header("音频是否循环播放")]
        public bool loop = false;
    }

    /// <summary> 存储所有音频信息
    public List<Sound> sounds;
    /// <summary>
    private Dictionary<string, AudioSource> audiosDic;

    private static AudioManger instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audiosDic = new Dictionary<string, AudioSource>();
        }
    }

    private void Start()
    {
        foreach (var sound in sounds)
        {
            GameObject obj = new GameObject(sound.clip.name);
            obj.transform.SetParent(transform);

            AudioSource source = obj.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.playOnAwake = sound.playOnAwake;
            source.loop = sound.loop;
            source.volume = sound.volume;
            source.outputAudioMixerGroup = sound.outputGroup;

            if (sound.playOnAwake)
            {
                source.Play();
            }
            audiosDic.Add(sound.clip.name, source);
        }
    }

    /// <summary> 播放音频
    public static void PlayAudio(string name, bool isWaiting = false)
    {
        if (!instance.audiosDic.ContainsKey(name))
        {
            Debug.LogWarning($"名为{name}的音频不存在！");
            return;
        }
        if (isWaiting)
        {
            if (!instance.audiosDic[name].isPlaying)
            {
                instance.audiosDic[name].Play();
            }
        }
        else
        {
            instance.audiosDic[name].Play();
        }
    }

    /// <summary> 关闭音频
    public static void StopAudio(string name)
    {
        if (!instance.audiosDic.ContainsKey(name))
        {
            Debug.LogWarning($"名为{name}的音频不存在！");
            return;
        }
        instance.audiosDic[name].Stop();
    }
}
