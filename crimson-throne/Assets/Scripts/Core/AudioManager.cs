using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance { get; private set; }

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
    #endregion

    #region Variables
    [Header("------------ Audio Settings ------------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [Header("------------ Audio Clip ------------")]
    [Header("Background")]
    [SerializeField] public AudioClip killBossVictory;
    [SerializeField] public AudioClip victoryResultsBackground;
    [SerializeField] public AudioClip meetTheBossFinalMapBackground;
    [SerializeField] public AudioClip chillMusicBackground;
    [SerializeField] public AudioClip daVinciBackground;
    [SerializeField] public AudioClip gameMusicLoopBackground;
    [SerializeField] public AudioClip gymPhonkBackground;
    [SerializeField] public AudioClip lofiOrchestraBackground;
    [SerializeField] public AudioClip vampireSoundtrackBackground;
    [SerializeField] public AudioClip resultsBackground;
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
    [SerializeField] public AudioClip bossDash;
    [SerializeField] public AudioClip bossFire;
    [SerializeField] public AudioClip enemyHurt;
    [Space(3)]
    [Header("Player")]
    [SerializeField] public AudioClip playerDie;
    [SerializeField] public AudioClip playerTakeDamage;
    #endregion

    #region Music Controls
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Music clip not found!");
            return;
        }
        musicSource.clip = clip;
        musicSource.Play();
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

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    #endregion

    #region SFX Controls
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("SFX clip not found!");
            return;
        }
        sfxSource.PlayOneShot(clip);
    }
    
    public void MuteSFX(bool mute)
    {
        AudioListener.volume = mute ? 0 : 1;
    }
    #endregion
}