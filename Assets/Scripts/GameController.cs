using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip goalAudioClip;

	[Space]
	[SerializeField] private ScreenFader screenFader;

	[Header("Scene Objects")]
	[SerializeField] private Amphora amphora;

	public static bool IsGameCompleted { get; private set; }

	public event Action WelcomeDismissed;

	private void Awake()
	{
		IsGameCompleted = false;

		amphora.Cracked += OnAmphoraCracked;
	}

	private void Start()
	{
		// TODO: move to moment when user continues from some welcome screen
		WelcomeDismissed?.Invoke();
	}

	private void OnDestroy()
	{
		amphora.Cracked -= OnAmphoraCracked;
	}

	private void Update()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.R))
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		if (Input.GetKeyDown(KeyCode.F))
			GameGoalAchieved();
#endif
	}

	private void OnAmphoraCracked()
	{
		if (Anup.IsRevertingActionOngoing)
			return;

		GameGoalAchieved();
	}

	private void GameGoalAchieved()
	{
		audioSource.PlayOneShot(goalAudioClip);

		IsGameCompleted = true;

		// TODO: Amphora cracked - game completed!
		// TODO: Some UI and on button StartOver
	}

	public void StartOver()
	{
		screenFader.FadeOut(() =>
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		});
	}
}
