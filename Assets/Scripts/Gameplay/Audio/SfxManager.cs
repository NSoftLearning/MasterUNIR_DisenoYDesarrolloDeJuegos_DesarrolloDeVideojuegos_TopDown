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
    [SerializeField] GameObject _audioSourcesPrefab;

    List<AudioSource> _audioSources = new List<AudioSource>();
    private void Awake()
    {
        for (int i = 0; i < _numOfAudioSources; i++)
        {
            GameObject newSource = Instantiate(_audioSourcesPrefab, transform);
            _audioSources.Add(newSource.GetComponent<AudioSource>());
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
}
