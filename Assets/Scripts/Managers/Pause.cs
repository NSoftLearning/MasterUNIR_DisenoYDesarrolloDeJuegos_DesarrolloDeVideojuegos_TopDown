using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class Pause : MonoBehaviour
{
    [Header("References")]
    [SerializeField] InputActionReference inputPausePlayer;
    [SerializeField] AudioSource musicAudio;
    [SerializeField] AudioSource sfxAudio;
    [SerializeField] AudioClip pauseClip;
    ScenesManager scenesManager;

    [Header("UI Pause")]
    [SerializeField] GameObject panelPause;

    [Header("Optional")]
    [SerializeField] bool isForCinematic = false;
    [SerializeField] PlayableDirector cinematic;

    private void OnEnable()
    {
        inputPausePlayer.action.Enable();
        inputPausePlayer.action.performed += OnPausePerformed;
    }

    private void Awake()
    {
        if (musicAudio == null)
        {
            musicAudio = GameObject.Find("MusicAudio").GetComponent<AudioSource>();
        }

        scenesManager = FindAnyObjectByType<ScenesManager>();

        sfxAudio = GetComponent<AudioSource>();
        sfxAudio.clip = pauseClip;

        panelPause = GameObject.Find("PanelPause");
        panelPause.SetActive(false);

        if (isForCinematic)
        {
            cinematic = FindAnyObjectByType<PlayableDirector>();
        }
    }

    void OnPausePerformed(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 1f && !scenesManager.onTransition)
        {
            Paused();
        }
        else if (Time.timeScale == 0f && !scenesManager.onTransition)
        {
            Resume();
        }
    }

    public void Paused()
    {
        panelPause.SetActive(true);
        Time.timeScale = 0f;
        sfxAudio.Play();
        Debug.Log("Juego pausado");

        if (isForCinematic)
        {
            cinematic.Pause();
        }
    }

    public void Resume()
    {
        panelPause.SetActive(false);
        Time.timeScale = 1f;
        sfxAudio.Play();
        Debug.Log("Juego reanudado");

        if (isForCinematic)
        {
            cinematic.Play();
        }

    }


}

