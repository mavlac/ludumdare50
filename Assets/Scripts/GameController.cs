using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip goalAudioClip;

	[Header("Scene Objects")]
	[SerializeField] private Amphora amphora;

	private void Awake()
	{
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
#endif
	}

	private void OnAmphoraCracked()
	{
		audioSource.PlayOneShot(goalAudioClip);

		if (Anup.IsRevertingActionOngoing)
			return;

		// TODO: Amphora cracked - game completed!
	}
}
