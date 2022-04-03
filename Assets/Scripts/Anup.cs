using System;
using System.Collections;
using UnityEngine;

public class Anup : MonoBehaviour
{
	[SerializeField] private Transform torso;
	[SerializeField] private GameObject armDefault;
	[SerializeField] private GameObject armAction0;
	[SerializeField] private GameObject armAction1;
	[SerializeField] private ParticleSystem magicParticles;

	[Space]
	[SerializeField] private Transform head;
	[SerializeField] private GameObject openedEye;

	[Space]
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip revertActionMagicAudioClip;

	[Header("Scene Objects")]
	[SerializeField] private Cat cat;
	[SerializeField] private Amphora amphora;

	Coroutine openEyeForCoroutine;

	public static bool IsRevertingActionOngoing { get; private set; }

	private void Awake()
	{
		SetDefaultPose();
		CloseEye();

		IsRevertingActionOngoing = false;
	}

	private void Start()
	{
		cat.PushPerformed += OnCatPushed;
	}

	private void Update()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.A))
			InitiateRevertAction();
#endif
	}

	private void OnDestroy()
	{
		cat.PushPerformed -= OnCatPushed;
	}

	private void OnCatPushed(Cat.PushPower pushPower)
	{
		switch(pushPower)
		{
			case Cat.PushPower.Low:
				OpenEyeFor(0.5f);
				break;

			case Cat.PushPower.Ideal:
				TurnHeadFor(preDelay: 0.5f, duration: 0.5f);
				break;

			case Cat.PushPower.Harsh:
				InitiateRevertAction();
				break;
		}
	}

	private void InitiateRevertAction()
	{
		StartCoroutine(RevertActionCoroutine());
	}
	IEnumerator RevertActionCoroutine()
	{
		IsRevertingActionOngoing = true;
		OpenEye();
		yield return new WaitForSeconds(0.25f);
		TurnHead(true);
		yield return new WaitForSeconds(0.15f);
		TurnTorso(true);
		yield return new WaitForSeconds(0.25f);
		SetArmAction(1);
		yield return new WaitForSeconds(0.25f);
		SetArmAction(2);

		magicParticles.Play();
		audioSource.PlayOneShot(revertActionMagicAudioClip);
		yield return new WaitForSeconds(0.25f);
		cat.ReturnToInitialPosition(0.75f);
		amphora.ReturnToInitialPosition(0.75f);
		yield return new WaitForSeconds(0.75f);

		magicParticles.Stop();
		SetArmAction(1);
		yield return new WaitForSeconds(0.25f);
		SetArmAction(0);
		CloseEye();
		yield return new WaitForSeconds(0.25f);
		TurnHead(false);
		yield return new WaitForSeconds(0.15f);
		TurnTorso(false);

		IsRevertingActionOngoing = false;
	}

	private void SetDefaultPose()
	{
		TurnHead(false);
		TurnTorso(false);
		SetArmAction(0);
	}

	private void SetArmAction(int step)
	{
		armDefault.SetActive(step == 0);
		armAction0.SetActive(step == 1);
		armAction1.SetActive(step == 2);
	}

	private void TurnHeadFor(float preDelay, float duration)
	{
		StartCoroutine(TurnHeadForCoroutine(preDelay, duration));
	}
	IEnumerator TurnHeadForCoroutine(float preDelay, float duration)
	{
		OpenEye();
		yield return new WaitForSeconds(preDelay);
		TurnHead(true);
		yield return new WaitForSeconds(duration - 0.25f);

		// Dont turn back if Amphora has been broken
		if (Amphora.IsPushedOverTableEdge)
			yield break;

		CloseEye();
		yield return new WaitForSeconds(0.25f);
		TurnHead(false);
	}

	private void TurnHead(bool value)
	{
		if (value)
			head.localScale = new Vector3(-1f, 1f, 1f);
		else
			head.localScale = Vector3.one;
	}

	private void TurnTorso(bool value)
	{
		if (value)
			torso.localScale = new Vector3(-1f, 1f, 1f);
		else
			torso.localScale = Vector3.one;
	}

	private void OpenEye()
	{
		openedEye.SetActive(true);
	}
	private void OpenEyeFor(float duration)
	{
		if (openEyeForCoroutine != null) StopCoroutine(openEyeForCoroutine);
		openEyeForCoroutine = StartCoroutine(OpenEyeForCoroutine(duration));
	}
	IEnumerator OpenEyeForCoroutine(float duration)
	{
		OpenEye();
		yield return new WaitForSeconds(duration);
		CloseEye();
		openEyeForCoroutine = null;
	}
	private void CloseEye()
	{
		openedEye.SetActive(false);
	}
}
