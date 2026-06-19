using System.Collections.Generic;
using UnityEngine;

public struct AudioInfo
{
    public AudioClip clip;
    public float volume;

    public AudioInfo(AudioClip clip, float volume)
    {
        this.clip = clip;
        this.volume = volume;
    }
}

public class SfxManager : MonoBehaviour
{
    [SerializeField] int _numOfAudioSources = 6;
    [SerializeField] int _numOfLoopAudioSources = 6;
    [SerializeField] GameObject _audioSourcesPrefab;

    List<AudioSource> _audioSources = new List<AudioSource>();
    List<AudioSource> _loopAudioSources = new List<AudioSource>();
    private void Awake()
    {
        for (int i = 0; i < _numOfAudioSources; i++)
        {
            GameObject newSource = Instantiate(_audioSourcesPrefab, transform);
            _audioSources.Add(newSource.GetComponent<AudioSource>());
            
        }

        for (int i = 0; i < _numOfLoopAudioSources; i++)
        {
            GameObject newSource = Instantiate(_audioSourcesPrefab, transform);
            AudioSource source = newSource.GetComponent<AudioSource>();
            source.loop = true;
            _loopAudioSources.Add(source);

        }
    }

    int index = 0;
    public void PlaySound(AudioInfo info)
    {
        AudioSource actSource = _audioSources[index];

        actSource.clip = info.clip;
        actSource.volume = info.volume;
        actSource.Play();

        index++;
        if (index == _numOfAudioSources) index = 0;
    }

    int loopIndex = 0;
    // Devuelve un id del indice para que se lo guarde quien lo llame y luego poder pararlo
    public int PlayLoopSound(AudioInfo info)
    {
        return 0;
    }
}
