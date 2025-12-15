using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ObscuringItemFader : MonoBehaviour
{
    private SpriteRenderer _sr;
    private float _currentAlpha = 1f;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        _currentAlpha = _sr.color.a;
        float distance = _currentAlpha - Settings.targetAlpha;

        while (_currentAlpha - Settings.targetAlpha > Mathf.Epsilon)
        {
            _currentAlpha -= distance / Settings.fadeOutSeconds * Time.deltaTime;
            _sr.color = new(1f, 1f, 1f, _currentAlpha);
            yield return null;
        }

        _sr.color = new(1f, 1f, 1f, Settings.targetAlpha);
    }

    private IEnumerator FadeInCoroutine()
    {
        _currentAlpha = _sr.color.a;
        float distance = 1f - _currentAlpha;

        while (1f - _currentAlpha > Mathf.Epsilon)
        {
            _currentAlpha += distance / Settings.fadeInSeconds * Time.deltaTime;
            _sr.color = new(1f, 1f, 1f, _currentAlpha);
            yield return null;
        }

        _sr.color = new(1f, 1f, 1f, 1f);
    }
}