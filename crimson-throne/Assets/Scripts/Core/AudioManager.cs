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
    [SerializeField] public AudioClip coinCollect;
    [SerializeField] public AudioClip healthCollect;
    [SerializeField] public AudioClip expCollect;
    [Space(3)]
    [Header("Abilities")]
    [SerializeField] public AudioClip katana;
    [SerializeField] public AudioClip hammer;
    [SerializeField] public AudioClip magicBall;
    [SerializeField] public AudioClip seekingBomb;
    [SerializeField] public AudioClip thunder;
    [SerializeField] public AudioClip tornado;
    [Space(3)]
    [Header("UI")]
    [SerializeField] public AudioClip buttonClick;
    [SerializeField] public AudioClip abilityChoose;
    [SerializeField] public AudioClip levelUpSound;
    [SerializeField] public AudioClip startNewGame;
    [Space(3)]
    [Header("Enemy")]
    [SerializeField] public AudioClip bossDeath;
    [SerializeField] public AudioClip enemyHurt;
    [Space(3)]
    [Header("Player")]
    [SerializeField] public AudioClip playerDie;
    [SerializeField] public AudioClip playerTakeDamage;

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

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }
}