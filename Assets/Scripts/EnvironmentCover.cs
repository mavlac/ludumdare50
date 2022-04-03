using UnityEngine;
using UnityEngine.Assertions;
using DG.Tweening;

public class EnvironmentCover : MonoBehaviour
{
	[SerializeField] private WelcomeScreen welcomeScreen;

	private SpriteRenderer spriteRenderer;
	private Vector3 initialPosition;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		Assert.IsNotNull(spriteRenderer);
		Assert.IsNotNull(welcomeScreen);

		initialPosition = transform.position;
		spriteRenderer.enabled = true;

		welcomeScreen.Dismissed += OnWelcomeDismissed;
	}

	private void Start()
	{
		if (WelcomeScreen.IsDismissed)
			spriteRenderer.enabled = false;
	}

	private void OnDestroy()
	{
		welcomeScreen.Dismissed -= OnWelcomeDismissed;
	}

	private void OnWelcomeDismissed()
	{
		Open();
	}

	public void Open()
	{
		transform.DOMoveY(initialPosition.y - 3f, 0.5f).SetEase(Ease.InCubic).OnComplete(() =>
		{
			spriteRenderer.enabled = false;
		});
	}
}
