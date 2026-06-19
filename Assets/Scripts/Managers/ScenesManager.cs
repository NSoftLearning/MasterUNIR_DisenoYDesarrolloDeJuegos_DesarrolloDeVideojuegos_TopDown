using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScenesManager : MonoBehaviour
{
    [Header("Canvas UI")]
    [SerializeField] CanvasGroup fadeCanvasGroup;

    [Header("Fade Settings")]
    [SerializeField] float fadeDuration = 1f;

    public bool onTransition = false;

    private void Awake()
    {
        fadeCanvasGroup = GameObject.Find("FadePanel").GetComponent<CanvasGroup>();
    }

    void Start()
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f; // Asegura que el panel de fade esté completamente invisible al inicio
            StartCoroutine(FadeIn());
        }
    }

    public void CallFadeOut_LoadScene(string nameScene)
    {
        if (fadeCanvasGroup != null)
        {
            StartCoroutine(FadeOut(nameScene));
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator FadeIn()
    {
        fadeCanvasGroup.blocksRaycasts = true; // Bloquea la interacción mientras se realiza el fade
        onTransition = true;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.blocksRaycasts = false; // Desbloquea la interacción cuando termina el fade
        onTransition = false;
    }

    IEnumerator FadeOut(string nameScene)
    {
        fadeCanvasGroup.blocksRaycasts = true;
        onTransition = true;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;

        SceneManager.LoadScene(nameScene);

    }
}
