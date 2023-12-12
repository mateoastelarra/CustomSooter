using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SoundSO : ScriptableObject
{
    public enum AudioTypes
    {
        SFX,
        Music
    }

    public AudioTypes AudioType;
    public AudioClip Clip;
    public bool Loop = false;
    public bool RandomizePitch = false;
    [Range(0f,1f)]
    public float RandomPitchModifier = 0.8f;
    [Range(0f, 2f)]
    public float Volume = 1f;
    [Range(0f, 2f)]
    public float Pitch = 1f;
}
