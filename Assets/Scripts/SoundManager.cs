using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("SFX Clips")]
    [SerializeField] private AudioClip clickSfx;
    [SerializeField] private AudioClip matchSfx;
    [SerializeField] private AudioClip mismatchSfx;
    [SerializeField] private AudioClip victorySfx;

    private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        sfxSource = GetComponent<AudioSource>();
        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.spatialBlend = 0f; 
        sfxSource.volume = 1f;
    }

    public void PlayClick()    { if (clickSfx    != null) sfxSource.PlayOneShot(clickSfx); }
    public void PlayMatch()    { if (matchSfx    != null) sfxSource.PlayOneShot(matchSfx); }
    public void PlayMismatch() { if (mismatchSfx != null) sfxSource.PlayOneShot(mismatchSfx); }
    public void PlayVictory()  { if (victorySfx  != null) sfxSource.PlayOneShot(victorySfx); }
}
