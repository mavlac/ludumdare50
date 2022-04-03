using System;
using DG.Tweening;
using UnityEngine;

public class Cat : MonoBehaviour
{
	public enum PushPower { Low, Ideal, Harsh }

	[Space]
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip pushLowAudioClip;
	[SerializeField] private AudioClip pushIdealAudioClip;
	[SerializeField] private AudioClip pushHarshAudioClip;

	private Vector3 initialPosition;

	public event Action<PushPower> PushPerformed;

	private void Awake()
	{
		initialPosition = transform.position;
	}

	private void Update()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.P))
		{
			Push(Input.GetKey(KeyCode.LeftShift) ? PushPower.Harsh : PushPower.Ideal);
		}
		if (Input.GetKeyDown(KeyCode.O))
		{
			Push(PushPower.Low);
		}
#endif
	}

	public void ReturnToInitialPosition(float duration)
	{
		transform.DOMove(initialPosition, duration).SetEase(Ease.InOutCubic);
	}

	private void Push(PushPower pushPower)
	{
		Debug.Log("Cat push " + pushPower);

		if (Anup.IsRevertingActionOngoing)
			return;

		PlayPushAudio(pushPower);

		PushPerformed?.Invoke(pushPower);
	}

	private void PlayPushAudio(PushPower pushPower)
	{
		switch(pushPower)
		{
			case PushPower.Low:
				audioSource.PlayOneShot(pushLowAudioClip);
				break;
			case PushPower.Ideal:
				audioSource.PlayOneShot(pushIdealAudioClip);
				break;
			case PushPower.Harsh:
				audioSource.PlayOneShot(pushHarshAudioClip);
				break;
		}
	}
}
