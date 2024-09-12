using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    private void Start(){
        PlayMusic("MainMenuMusic");
    }
    
    public void PlayMusic(string clipName){
        AudioClip clip = Resources.Load<AudioClip>(clipName);
        if(clip != null){
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string clipName){
        AudioClip clip = Resources.Load<AudioClip>(clipName);
        if(clip != null){
            sfxSource.PlayOneShot(clip);
        }
    }

    public void SetMusicVolume(float sliderValue)
    {
        float volume = Mathf.Clamp(sliderValue / 100.0f, 0.0001f, 1.0f);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float sliderValue)
    {
        float volume = Mathf.Clamp(sliderValue / 100.0f, 0.0001f, 1.0f);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }
    public void ToggleMusic(){
        //musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX(){
        //sfxSource.mute = !sfxSource.mute;
    }
    
    
    
}