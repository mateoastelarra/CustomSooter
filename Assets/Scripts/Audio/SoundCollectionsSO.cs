
using UnityEngine;

[CreateAssetMenu()]
public class SoundCollectionsSO : ScriptableObject
{
    [Header("SFX")]
    public SoundSO[] GunShoot;
    public SoundSO[] Jump;
    public SoundSO[] Splat;
    public SoundSO[] Jetpack;

    [Header("Music")]
    public SoundSO[] FightMusic;
    public SoundSO[] DiscoPartyMusic;
}
