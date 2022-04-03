using System.Collections;
using UnityEngine;

public class Raise : MonoBehaviour
{
	const float RaiseSpeed = 2f;

	[SerializeField] private GameObject hint;

	[Space]
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip activateAudioClip;
	[SerializeField] private AudioClip releaseAudioClip;

	[Space]
	[SerializeField] private Cat cat;

	private bool IsReadyToBegin =>
		!IsActive &&
		!Anup.IsRevertingActionOngoing &&
		!Amphora.IsPushedOverTableEdge &&
		!Amphora.IsCracked &&
		!Cat.IsPushAndMoveInProgress &&
		!ScreenFader.IsInProgress &&
		!GameController.IsGameCompleted &&
		WelcomeScreen.IsDismissed;

	private bool wasReadyToBegin;

	private float timeSinceActivation;

	public bool IsActive { get; private set; }
	public bool IsReleasing { get; private set; }
	public float NormalizedPushPower { get; private set; }

	private void Awake()
	{
		ActivateReadyHint(false);
		IsActive = false;
		IsReleasing = false;
	}

	private void Update()
	{
		timeSinceActivation += Time.deltaTime;

		if (IsReadyToBegin && !wasReadyToBegin && !GameController.IsGameCompleted)
		{
			ActivateReadyHint(true);
		}


		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
		{
			if (IsReadyToBegin)
				Activate();
		}
		else if (IsActive && !IsReleasing)
		{
			if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.Space) && !Input.GetMouseButton(0))
			{
				Release();
			}
			else
			{
				// Being active - change the value
				// Pure sin
				NormalizedPushPower = 0.5f + Mathf.Sin(timeSinceActivation * RaiseSpeed) * 0.5f;
				// Shake
				float shake = Mathf.Sin(timeSinceActivation * 7f) * 0.1f;
				float shakeAmount = (0.5f - Mathf.Abs(NormalizedPushPower - 0.5f)) * 2f;
				NormalizedPushPower = Mathf.Clamp01(NormalizedPushPower + shake * shakeAmount);
			}
		}


		wasReadyToBegin = IsReadyToBegin;
	}

	private void Activate()
	{
		IsActive = true;

		ActivateReadyHint(false);

		audioSource.PlayOneShot(activateAudioClip);

		NormalizedPushPower = 0f;

		if (Random.value < 0.5f)
			// Starting at 0
			timeSinceActivation = -Mathf.PI * 0.5f / RaiseSpeed;
		else
			// Starting at 1
			timeSinceActivation = Mathf.PI * 0.5f / RaiseSpeed;
	}
	private void Release()
	{
		IsReleasing = true;

		var power = Cat.PushPower.Ideal;
		if (NormalizedPushPower < 0.4f)
			power = Cat.PushPower.Low;
		if (NormalizedPushPower > 0.6f)
			power = Cat.PushPower.Harsh;
		cat.Push(power);
		
		StartCoroutine(ReleasedDelayedDeactivateCoroutine());

		//audioSource.PlayOneShot(releaseAudioClip);
	}
	IEnumerator ReleasedDelayedDeactivateCoroutine()
	{
		yield return new WaitForSeconds(0.15f);
		IsActive = false;
		IsReleasing = false;
	}

	private void ActivateReadyHint(bool value)
	{
		hint.SetActive(value);
	}
}
