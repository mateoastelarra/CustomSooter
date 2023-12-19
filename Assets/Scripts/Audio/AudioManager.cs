using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Range(0,2)]
    [SerializeField] float _masterVolume = 1f;
    [SerializeField] float _destroyTimeBonus = 0.1f; // So that sound doesnt clips when destroyed.
    [SerializeField] private SoundCollectionsSO _soundCollectionsSO;
    [SerializeField] private AudioMixerGroup _musicMixerGroup;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;

    private AudioSource _currentMusic;

    #region Unity Methods

    private void Awake()
    {
        if (Instance == null) { Instance = this; }    
    }

    private void Start()
    {
        FightMusic();
    }

    private void OnEnable()
    {
        Gun.OnShoot += Gun_OnShoot;
        Gun.OnLaunchGrenade += Gun_OnLaunchGrenade;
        PlayerController.OnJump += PlayerController_OnJump;
        PlayerController.OnJetpack += PlayerController_OnJetpack;
        PlayerHit.OnPlayerHit += PlayerController_OnPlayerHit;
        Health.OnDeath += Health_OnDeath;
        DiscoballManager.OnStartParty += DiscoPartyMusic;
        DiscoballManager.OnFinishParty += FightMusic;
    }

    private void OnDisable()
    {
        Gun.OnShoot -= Gun_OnShoot;
        Gun.OnLaunchGrenade -= Gun_OnLaunchGrenade;
        PlayerController.OnJump -= PlayerController_OnJump;
        PlayerController.OnJetpack -= PlayerController_OnJetpack;
        PlayerHit.OnPlayerHit -= PlayerController_OnPlayerHit;
        Health.OnDeath -= Health_OnDeath;
        DiscoballManager.OnStartParty -= DiscoPartyMusic;
        DiscoballManager.OnFinishParty -= FightMusic;
    }
    #endregion

    #region Sound Methods
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
        pitch = RandomizePitch(soundSO, pitch);
        AudioMixerGroup audioMixerGroup = DetermineAudioMixerGroup(soundSO);

        PlaySound(clip, volume, pitch, loop, audioMixerGroup);
    }

    private AudioMixerGroup DetermineAudioMixerGroup(SoundSO soundSO)
    {
        AudioMixerGroup audioMixerGroup = null;
        if (soundSO.AudioType == SoundSO.AudioTypes.Music)
        {
            audioMixerGroup = _musicMixerGroup;
        }
        else if (soundSO.AudioType == SoundSO.AudioTypes.SFX)
        {
            audioMixerGroup = _sfxMixerGroup;
        }

        return audioMixerGroup;
    }

    private float RandomizePitch(SoundSO soundSO, float pitch)
    {
        if (soundSO.RandomizePitch)
        {
            float randomPitchModifier = Random.Range(-soundSO.RandomPitchModifier, soundSO.RandomPitchModifier);
            pitch += randomPitchModifier;
        }

        return pitch;
    }

    private void PlaySound(AudioClip clip, float volume, float pitch, bool loop, AudioMixerGroup audioMixerGroup)
    {
        GameObject soundObject = new GameObject("Temp Audio Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.loop = loop;
        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.Play();

        if (!loop) { Destroy(soundObject, clip.length + _destroyTimeBonus); }

        DetermineMusic(audioMixerGroup, audioSource);
    }

    private void DetermineMusic(AudioMixerGroup audioMixerGroup, AudioSource audioSource)
    {
        if (audioMixerGroup == _musicMixerGroup)
        {
            if (_currentMusic != null) { _currentMusic.Stop(); }
            _currentMusic = audioSource;
        }
    }
    #endregion

    #region SFX
    private void Gun_OnShoot()
    {
        PlayRandomSound(_soundCollectionsSO.GunShoot);
    }

    private void PlayerController_OnJump()
    {
        PlayRandomSound(_soundCollectionsSO.Jump);
    }

    private void PlayerController_OnJetpack()
    {
        PlayRandomSound(_soundCollectionsSO.Jetpack);
    }

    private void PlayerController_OnPlayerHit()
    {
        PlayRandomSound(_soundCollectionsSO.PlayerHit);
    }

    private void Health_OnDeath(Health sender)
    {
        PlayRandomSound(_soundCollectionsSO.Splat);
    }

    private void Gun_OnLaunchGrenade()
    {
        PlayRandomSound(_soundCollectionsSO.GrenadeLaunch);
    }

    public void Grenade_OnTick()
    {
        PlayRandomSound(_soundCollectionsSO.GrenadeTick);
    }

    public void Grenade_OnExplode()
    {
        PlayRandomSound(_soundCollectionsSO.GrenadeExplode);
    }
    #endregion

    #region Music
    private void FightMusic()
    {
        PlayRandomSound(_soundCollectionsSO.FightMusic);
    }

    private void DiscoPartyMusic()
    {
        PlayRandomSound(_soundCollectionsSO.DiscoPartyMusic);
    }
    #endregion

}
