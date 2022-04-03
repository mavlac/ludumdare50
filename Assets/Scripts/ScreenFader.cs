using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
	[SerializeField] private Image image;
	[SerializeField] private float duration = 1f;

	private void Start()
	{
		FadeIn();
	}

	public void FadeIn()
	{
		image.color = image.color.WithAlpha(1f);
		Fade(0f);
	}

	public void FadeOut()
	{
		Fade(1f);
	}

	public void Fade(float targetAlpha)
	{
		StopAllCoroutines();
		StartCoroutine(FadeCoroutine(targetAlpha));
	}

	IEnumerator FadeCoroutine(float targetAlpha)
	{
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
	}
}
