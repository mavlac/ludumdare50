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

	private void Awake()
	{
		IsGameCompleted = false;

		amphora.Cracked += OnAmphoraCracked;
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
