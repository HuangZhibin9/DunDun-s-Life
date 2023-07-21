using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public void SetBGMVolume(float volume)
    {
        mixer.SetFloat("BGM", volume);
    }
    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat("SFX", volume);
    }
}
