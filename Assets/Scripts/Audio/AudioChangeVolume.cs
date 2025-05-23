using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioChangeVolume : MonoBehaviour
{
    public AudioMixer group;
    public string floatParam = "VolumeX";

    public void ChangeValue(float f)
    {
        group.SetFloat(floatParam, f);
    }
}
