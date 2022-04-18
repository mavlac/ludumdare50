using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Cat : MonoBehaviour
{
	public const float PushDistance = 0.1575f;

	public static bool IsPushAndMoveInProgress { get; private set; }

	public enum PushPower { Low, Ideal, Harsh }

	[SerializeField] private GameObject mainDefault;
	[SerializeField] private GameObject mainMove;
	[SerializeField] private GameObject armAction0;
	[SerializeField] private GameObject armAction1;
	[SerializeField] private GameObject headLeft;
	[SerializeField] private GameObject headFront;

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
		IsPushAndMoveInProgress = false;
	}

	private void Start()
	{
		LookLeft();
		SetArmAction(0);
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
		LookFront();
		transform.DOMove(initialPosition, duration).SetEase(Ease.InOutCubic).OnComplete(() =>
		{
			LookLeft();
		});
	}

	public void Push(PushPower pushPower)
	{
		//Debug.Log("Cat push " + pushPower);

		if (Anup.IsRevertingActionOngoing ||
			Amphora.IsPushedOverTableEdge)
			return;

		PlayPushAudio(pushPower);

		PushPerformed?.Invoke(pushPower);

		StartCoroutine(PushAnimationCoroutine(pushPower));
	}
	IEnumerator PushAnimationCoroutine(PushPower pushPower)
	{
		IsPushAndMoveInProgress = true;

		SetArmAction(1);
		yield return new WaitForSeconds(0.25f);
		SetArmAction(2);
		yield return new WaitForSeconds(0.25f);
		SetArmAction(0);

		if (pushPower != PushPower.Low)
		{
			if (Amphora.IsPushedOverTableEdge)
			{
				// All done, no need to move the cat further anymore
				IsPushAndMoveInProgress = false;
			}
			else
			{
				BodyMoving();
				transform.DOMoveX(transform.position.x - PushDistance, 0.5f).SetEase(Ease.InOutCubic).OnComplete(() =>
				{
					BodySitting();
					IsPushAndMoveInProgress = false;
				});
			}
		}
		else
		{
			yield return new WaitForSeconds(0.1f);
			IsPushAndMoveInProgress = false;
		}
	}

	private void LookLeft()
	{
		headLeft.SetActive(true);
		headFront.SetActive(false);
	}
	private void LookFront()
	{
		headLeft.SetActive(false);
		headFront.SetActive(true);
	}

	private void BodySitting()
	{
		mainDefault.SetActive(true);
		mainMove.SetActive(false);
	}
	private void BodyMoving()
	{
		mainDefault.SetActive(false);
		mainMove.SetActive(true);
	}

	private void SetArmAction(int step)
	{
		armAction0.SetActive(step == 1);
		armAction1.SetActive(step == 2);
	}

	private void PlayPushAudio(PushPower pushPower)
	{
		switch(pushPower)
		{
			case PushPower.Low:
				audioSource.PlayOneShot(pushLowAudioClip);
				break;
			case PushPower.Ideal:
				audioSource.PlayOneShot(pushLowAudioClip);
				audioSource.PlayOneShot(pushIdealAudioClip);
				break;
			case PushPower.Harsh:
				audioSource.PlayOneShot(pushHarshAudioClip);
				break;
		}
	}
}
