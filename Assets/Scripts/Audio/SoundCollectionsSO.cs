
using UnityEngine;

[CreateAssetMenu()]
public class SoundCollectionsSO : ScriptableObject
{
    [Header("SFX")]
    public SoundSO[] GunShoot;
    public SoundSO[] Jump;
    public SoundSO[] Splat;
    public SoundSO[] Jetpack;
    public SoundSO[] GrenadeLaunch;
    public SoundSO[] GrenadeTick;
    public SoundSO[] GrenadeExplode;

    [Header("Music")]
    public SoundSO[] FightMusic;
    public SoundSO[] DiscoPartyMusic;
}
