using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    public Image fadeImage;
    public float duracionFade = 1f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    // 🌅 FADE IN (de negro a transparente)
    IEnumerator FadeIn()
    {
        float t = 0f;

        Color color = fadeImage.color;

        while (t < duracionFade)
        {
            t += Time.deltaTime;
            color.a = 1 - (t / duracionFade);
            fadeImage.color = color;

            yield return null;
        }

        color.a = 0;
        fadeImage.color = color;
    }

    // 🌑 FADE OUT (de transparente a negro)
    public void FadeOut(string escena)
    {
        StartCoroutine(FadeOutCoroutine(escena));
    }

    IEnumerator FadeOutCoroutine(string escena)
    {
        float t = 0f;

        Color color = fadeImage.color;

        while (t < duracionFade)
        {
            t += Time.deltaTime;
            color.a = t / duracionFade;
            fadeImage.color = color;

            yield return null;
        }

        color.a = 1;
        fadeImage.color = color;

        SceneManager.LoadScene(escena);
    }
}