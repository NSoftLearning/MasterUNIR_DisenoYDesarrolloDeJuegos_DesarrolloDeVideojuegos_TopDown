using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Awake()
    {
        if (masterSlider == null)
        {
            masterSlider = GameObject.Find("SliderMaster").GetComponent<Slider>();
        }
        if (musicSlider == null)
        {
            musicSlider = GameObject.Find("SliderMusic").GetComponent<Slider>();

        }
        if (sfxSlider == null)
        {
            sfxSlider = GameObject.Find("SliderSFX").GetComponent<Slider>();

        }

    }

    void Start()
    {
        // Cargar valores guardados
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

        masterSlider.value = master;
        musicSlider.value = music;
        sfxSlider.value = sfx;

        // Aplicar valores
        AudioManager.instance.SetMasterVolume(master);
        AudioManager.instance.SetMusicVolume(music);
        AudioManager.instance.SetSFXVolume(sfx);

        // El audio Manager escucha los cambios hechos y los aplica al audiomixer, guardando tmb los datos en PlayerPrefs
        masterSlider.onValueChanged.AddListener(AudioManager.instance.SetMasterVolume);
        musicSlider.onValueChanged.AddListener(AudioManager.instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.instance.SetSFXVolume);
    }

    
}
