using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip goalAudioClip;

	[Space]
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
	}
}
