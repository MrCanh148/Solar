using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] musicSound, sfxSound;
    public AudioSource musicSource, sfxSource;
    public AudioMixer myMixer;

    private Sound s;

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

    public void PlayMusic(string name)
    {
        s = Array.Find(musicSound, x => x.name == name);
        musicSource.clip = s.clip;
        musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        s = Array.Find(sfxSound, x => x.name == name);
        sfxSource.PlayOneShot(s.clip);
    }

    public void StopMusic(string name)
    {
        s = Array.Find(musicSound, x => x.name == name);
        musicSource.clip = s.clip;
        musicSource.Stop();
    }
}