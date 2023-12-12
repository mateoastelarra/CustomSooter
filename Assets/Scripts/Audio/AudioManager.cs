using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] float _masterVolume = 1f;
    [SerializeField] float _destroyTimeBonus = 0.1f; // So that sound doesnt clips when destroyed.
    [SerializeField] private SoundCollectionsSO _soundCollectionsSO;

    private void OnEnable()
    {
        Gun.OnShoot += Gun_OnShoot;
        PlayerController.OnJump += PlayerController_OnJump;
    }

    private void OnDisable()
    {
        Gun.OnShoot -= Gun_OnShoot;
        PlayerController.OnJump -= PlayerController_OnJump;
    }

    private void PlayRandomSound(SoundSO[] sounds)
    {
        if (sounds != null && sounds.Length > 0)
        {
            SoundSO soundSO = sounds[Random.Range(0, sounds.Length)];
            SoundToPlay(soundSO);
        }
    }

    private void SoundToPlay(SoundSO soundSO)
    {
        AudioClip clip = soundSO.Clip;
        float volume = soundSO.Volume * _masterVolume;
        float pitch = soundSO.Pitch;
        bool loop = soundSO.Loop;

        if (soundSO.RandomizePitch)
        {
            float randomPitchModifier = Random.Range(- soundSO.RandomPitchModifier, soundSO.RandomPitchModifier);
            pitch += randomPitchModifier;
        }

        PlaySound(clip, volume, pitch, loop);
    }

    private void PlaySound(AudioClip clip, float volume, float pitch, bool loop)
    {
        GameObject soundObject = new GameObject("Temp Audio Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.loop = loop;
        audioSource.Play();

        if (!loop) { Destroy(soundObject, clip.length + _destroyTimeBonus); }
    }

    private void Gun_OnShoot()
    {
        PlayRandomSound(_soundCollectionsSO.GunShoot);
    }

    private void PlayerController_OnJump()
    {
        PlayRandomSound(_soundCollectionsSO.Jump);
    }
}
