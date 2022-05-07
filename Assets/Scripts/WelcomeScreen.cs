using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeScreen : MonoBehaviour
{
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioSource musicAudioSource;
	[SerializeField] private AudioClip buttonClickedAudioClip;
	
	[Space]
	[SerializeField] private Button playButton;
	[SerializeField] private GameObject glyphs;

	public static bool IsDismissed { get; private set; } = false;

	public event Action Dismissed;

	private void Awake()
	{
		playButton.onClick.AddListener(OnPlayButtonClicked);

		if (IsDismissed)
			Hide();
		
		glyphs.gameObject.SetActive(false);
	}

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(1f);
		if (!IsDismissed)
			musicAudioSource	.Play();

		yield return new WaitForSeconds(0.5f);
		if (!IsDismissed)
			glyphs.gameObject.SetActive(true);
	}

	private void OnPlayButtonClicked()
	{
		if (IsDismissed)
			return;

		audioSource.PlayOneShot(buttonClickedAudioClip);

		Dismissed?.Invoke();
		IsDismissed = true;
		musicAudioSource.Stop();

		Hide();
	}

	private void Hide()
	{
		playButton.gameObject.SetActive(false);
		glyphs.gameObject.SetActive(false);
	}
}
