using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
	public static bool IsInProgress { get; private set; }

	[SerializeField] private Image image;
	[SerializeField] private float duration = 1f;

	[SerializeField] private Color[] colors;

	private float StepDuration => duration / colors.Length;

	private void Start()
	{
		FadeIn();
	}

	public void FadeIn(Action OnCompleted = null)
	{
		Fade(true, OnCompleted);
	}

	//public void FadeOut(Action OnCompleted = null)
	//{
	//	Fade(false, OnCompleted);
	//}

	public void Fade(bool fadeIn, Action OnCompleted)
	{
		StopAllCoroutines();
		StartCoroutine(FadeCoroutine(fadeIn, OnCompleted));
	}

	IEnumerator FadeCoroutine(bool fadeIn, Action OnCompleted)
	{
		IsInProgress = true;
		image.enabled = true;
		
		for (int step = 0; step < colors.Length; step++)
		{
			image.color = fadeIn ? colors[step] : colors[colors.Length - step - 1];
			yield return new WaitForSeconds(StepDuration);
		}

		image.enabled = false;
		IsInProgress = false;
		OnCompleted?.Invoke();
	}
}
