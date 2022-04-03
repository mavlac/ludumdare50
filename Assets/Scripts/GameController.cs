using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	[SerializeField] private AudioSource audioSource;

	[Space]
	[SerializeField] private ScreenFader screenFader;

	[Space]
	[SerializeField] private WelcomeScreen welcomeScreen;

	[Header("Scene Objects")]
	[SerializeField] private Amphora amphora;
	[SerializeField] private GameObject raiseHint;

	public static bool IsGameCompleted { get; private set; }

	public event Action GameCompleted;

	private void Awake()
	{
		IsGameCompleted = false;

		welcomeScreen.Dismissed += OnWelcomeScreenDismissed;

		amphora.Cracked += OnAmphoraCracked;
	}

	private void OnDestroy()
	{
		welcomeScreen.Dismissed -= OnWelcomeScreenDismissed; 
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

	private void OnWelcomeScreenDismissed()
	{

	}

	private void OnAmphoraCracked()
	{
		GameGoalAchieved();
	}

	private void GameGoalAchieved()
	{
		raiseHint.SetActive(false);

		IsGameCompleted = true;

		Debug.Log("Game Completed");

		GameCompleted?.Invoke();
	}
}
