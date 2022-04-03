using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
	public static bool IsInProgress { get; private set; }

	[SerializeField] private Image image;
	[SerializeField] private float duration = 1f;

	private void Start()
	{
		FadeIn();
	}

	public void FadeIn(Action OnCompleted = null)
	{
		image.color = image.color.WithAlpha(1f);
		Fade(0f, OnCompleted);
	}

	public void FadeOut(Action OnCompleted = null)
	{
		Fade(1f, OnCompleted);
	}

	public void Fade(float targetAlpha, Action OnCompleted)
	{
		StopAllCoroutines();
		StartCoroutine(FadeCoroutine(targetAlpha, OnCompleted));
	}

	IEnumerator FadeCoroutine(float targetAlpha, Action OnCompleted)
	{
		IsInProgress = true;

		image.enabled = true;
		float startAlpha = image.color.a;

		for (float t = 0f; t < duration; t += Time.deltaTime)
		{
			float alpha = Mathf.Lerp(startAlpha, targetAlpha, t / duration);
			image.color = image.color.WithAlpha(alpha);
			yield return null;
		}

		image.color = image.color.WithAlpha(targetAlpha);
		if (targetAlpha == 0f)
			image.enabled = false;

		OnCompleted?.Invoke();
		IsInProgress = false;
	}
}
