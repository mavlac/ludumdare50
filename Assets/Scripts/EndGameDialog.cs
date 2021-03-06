using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class EndGameDialog : MonoBehaviour
{
	[SerializeField] private GameController gameController;
	[SerializeField] private ScreenFader screenFader;

	[Space]
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip goalAudioClip;
	[SerializeField] private AudioClip buttonClickedAudioClip;

	[Space]
	[SerializeField] private GameObject dialog;
	[SerializeField] private RectTransform dialogRectTransform;
	[SerializeField] private Button replayButton;
	[SerializeField] private GameObject glyphs;
	[SerializeField] private GameObject bastet;

	private Vector2 initialDialogSize;

	private void Awake()
	{
		gameController.GameCompleted += OnGameCompleted;
		replayButton.onClick.AddListener(OnReplayButtonClicked);
		dialog.SetActive(false);
		replayButton.gameObject.SetActive(false);
		glyphs.SetActive(false);
		bastet.SetActive(false);
	}

	private void OnDestroy()
	{
		gameController.GameCompleted -= OnGameCompleted;
		replayButton.onClick.RemoveListener(OnReplayButtonClicked);
	}

	private void OnGameCompleted()
	{
		StartCoroutine(ShowEndDialogCoroutine());
	}
	IEnumerator ShowEndDialogCoroutine()
	{
		yield return new WaitForSeconds(2f);
		audioSource.PlayOneShot(goalAudioClip);
		dialog.SetActive(true);
		initialDialogSize = dialogRectTransform.sizeDelta;
		dialogRectTransform.sizeDelta = new Vector2(initialDialogSize.x, 12f);
		yield return new WaitForSeconds(0.1f);
		dialogRectTransform.sizeDelta = new Vector2(initialDialogSize.x, initialDialogSize.y * 0.5f);
		yield return new WaitForSeconds(0.1f);
		dialogRectTransform.sizeDelta = initialDialogSize;
		glyphs.SetActive(true);
		bastet.SetActive(true);

		yield return new WaitForSeconds(0.25f);
		replayButton.gameObject.SetActive(true);
	}

	private void OnReplayButtonClicked()
	{
		if (ScreenFader.IsInProgress)
			return;

		audioSource.PlayOneShot(buttonClickedAudioClip);

		StartCoroutine(StartOverCoroutine());
	}

	IEnumerator StartOverCoroutine()
	{
		yield return new WaitForSeconds(0.1f);
		replayButton.gameObject.SetActive(false);
		glyphs.SetActive(false);
		bastet.SetActive(false);
		yield return new WaitForSeconds(0.25f);
		dialogRectTransform.sizeDelta = new Vector2(initialDialogSize.x, initialDialogSize.y * 0.5f);
		yield return new WaitForSeconds(0.1f);
		dialog.SetActive(false);

		yield return new WaitForSeconds(0.5f);

		StartOver();
	}
	private void StartOver()
	{
		//screenFader.FadeOut(() =>
		//{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		//});
	}
}
