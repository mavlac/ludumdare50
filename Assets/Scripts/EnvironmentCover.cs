using UnityEngine;
using UnityEngine.Assertions;
using DG.Tweening;

public class EnvironmentCover : MonoBehaviour
{
	[SerializeField] private GameController gameController;

	private SpriteRenderer spriteRenderer;
	private Vector3 initialPosition;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		Assert.IsNotNull(spriteRenderer);
		Assert.IsNotNull(gameController);

		initialPosition = transform.position;
		spriteRenderer.enabled = true;

		gameController.WelcomeDismissed += OnWelcomeDismissed;
	}

	private void OnDestroy()
	{
		gameController.WelcomeDismissed -= OnWelcomeDismissed;
	}

	private void OnWelcomeDismissed()
	{
		Open();
	}

	public void Open()
	{
		transform.DOMoveY(transform.position.y - 3f, 0.5f).SetEase(Ease.InCubic).OnComplete(() =>
		{
			spriteRenderer.enabled = false;
		});
	}
}
