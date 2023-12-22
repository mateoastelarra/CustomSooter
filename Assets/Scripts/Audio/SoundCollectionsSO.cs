
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
    public SoundSO[] PlayerHit;
    public SoundSO[] MegaKill;

    [Header("Music")]
    public SoundSO[] FightMusic;
    public SoundSO[] DiscoPartyMusic;
}
