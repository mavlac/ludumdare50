using UnityEngine;
using UnityEngine.Assertions;

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
		// TODO: slide the cover out and disable when completed
		spriteRenderer.enabled = false;
	}
}
