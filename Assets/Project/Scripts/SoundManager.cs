using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    // inspectordan atayacağım
    [SerializeField] private AudioSource sfxSource;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip clickSfx;
    [SerializeField] private AudioClip matchSfx;
    [SerializeField] private AudioClip mismatchSfx;
    [SerializeField] private AudioClip victorySfx;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
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