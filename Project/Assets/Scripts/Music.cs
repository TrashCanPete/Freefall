﻿using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Music
{
    public string name;


    public AudioClip clip;

    [Range(0f, 1f)]
    public float volumn;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;
    public bool mute;

    [HideInInspector]
    public AudioSource source;
}
