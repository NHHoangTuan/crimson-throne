using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [Header("------------ Audio Source ------------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("------------ Audio Clip ------------")]
    [Header("Background")]
    [SerializeField] public AudioClip chillMusicBackground;
    [SerializeField] public AudioClip daVinciBackground;
    [SerializeField] public AudioClip gameMusicLoopBackground;
    [SerializeField] public AudioClip gymPhonkBackground;
    [SerializeField] public AudioClip lofiOrchestraBackground;
    [SerializeField] public AudioClip vampireSoundtrackBackground;
    [Space(3)]
    [Header("Items")]
    [Space(3)]
    [Header("Enemy")]
    [Space(3)]
    [Header("Player")]
    [SerializeField] public AudioClip levelUpSound;
    [SerializeField] public AudioClip healSound;
    [SerializeField] public AudioClip gameOverSound;

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSource.clip = lofiOrchestraBackground;
        musicSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}