using UnityEngine;

public class SoundManager : SingletonBehaviour<SoundManager>
{

    [Header("Audio Sources")]
    // inspectordan atayacağım
    [SerializeField] private AudioSource sfxSource;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip clickSfx;
    [SerializeField] private AudioClip matchSfx;
    [SerializeField] private AudioClip mismatchSfx;
    [SerializeField] private AudioClip victorySfx;


    protected override void Awake()
    {
        base.Awake();
    }
    public void PlayClick()
    {
        sfxSource.PlayOneShot(clickSfx);
    }
    public void PlayMatch()
    {
        sfxSource.PlayOneShot(matchSfx);
    }
    public void PlayMismatch()
    {
        sfxSource.PlayOneShot(mismatchSfx);
    }
    public void PlayVictory()
    {
        sfxSource.PlayOneShot(victorySfx);
    }
}