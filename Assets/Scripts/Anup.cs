using System.Collections;
using UnityEngine;

public class Anup : MonoBehaviour
{
	[SerializeField] private Transform torso;
	[SerializeField] private GameObject armDefault;
	[SerializeField] private GameObject armAction0;
	[SerializeField] private GameObject armAction1;

	[SerializeField] private Transform head;
	[SerializeField] private GameObject openedEye;

	Coroutine openEyeForCoroutine;

	private void Awake()
	{
		SetDefaultPose();
		CloseEye();
	}

	private void SetDefaultPose()
	{
		TurnHead(false);
		TurnTorso(false);
		armDefault.SetActive(true);
		armAction0.SetActive(false);
		armAction1.SetActive(false);
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
