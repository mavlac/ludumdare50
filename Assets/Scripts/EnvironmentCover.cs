using UnityEngine;
using UnityEngine.Assertions;
using DG.Tweening;
using System.Collections;

public class EnvironmentCover : MonoBehaviour
{
	[SerializeField] private WelcomeScreen welcomeScreen;

	[SerializeField] private int steps;
	[SerializeField] private float duration;

	[SerializeField] private SpriteRenderer spriteRenderer;

	private Vector3 initialPosition;

	private void Awake()
	{
		Assert.IsNotNull(spriteRenderer);
		Assert.IsNotNull(welcomeScreen);

		initialPosition = transform.position;
		spriteRenderer.enabled = true;

		welcomeScreen.Dismissed += OnWelcomeDismissed;
	}

	private void Start()
	{
		if (WelcomeScreen.IsDismissed)
		{
			spriteRenderer.enabled = false;
		}
	}

	private void OnDestroy()
	{
		welcomeScreen.Dismissed -= OnWelcomeDismissed;
	}

	private void OnWelcomeDismissed()
	{
		StopAllCoroutines();
		StartCoroutine(OpenCoroutine());
	}

	IEnumerator OpenCoroutine()
	{
		var targetPosition = initialPosition + Vector3.down * 3f;

		for (int i = 0; i < steps; i++)
		{
			var position = Vector3.Lerp(initialPosition, targetPosition, Mathf.InverseLerp(0, steps, i));
			transform.position = position;
			yield return new WaitForSeconds(duration / steps);
		}
		
		spriteRenderer.enabled = false;
	}
}
