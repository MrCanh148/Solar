using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slider_MusicSFX : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI Music, SFX;
    private float volume, vol;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
            LoadVolume();
        else
        {
            SetMusicVolume();
            SetSfxVolume();
        }
    }

    private void Update()
    {
        if (musicSlider.value == 0)
            Music.text = "music [off]";
        else
            Music.text = "music [on]";

        if (sfxSlider.value == 0)
            SFX.text = "sfx [off]";
        else
            SFX.text = "sfx [on]";
    }

    public void SetMusicVolume()
    {
        volume = musicSlider.value;
        if (volume == 0)
        {
            AudioManager.instance.myMixer.SetFloat("Music", -80);
        }
        else
        {
            AudioManager.instance.myMixer.SetFloat("Music", Mathf.Log10(volume) * 20); 
        }
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSfxVolume()
    {
        vol = sfxSlider.value;
        if (vol == 0)
        {
            AudioManager.instance.myMixer.SetFloat("Sfx", -80); 
        }
        else
        {
            AudioManager.instance.myMixer.SetFloat("Sfx", Mathf.Log10(vol) * 20); 
        }
        PlayerPrefs.SetFloat("sfxVolume", vol);
    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetMusicVolume();
        SetSfxVolume();
    }
}
