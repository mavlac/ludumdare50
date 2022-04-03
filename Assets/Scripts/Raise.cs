using System.Collections;
using UnityEngine;

public class Raise : MonoBehaviour
{
	[SerializeField] private GameObject hint;

	[Space]
	[SerializeField] private Cat cat;

	private bool IsReadyToBegin =>
		!IsActive &&
		!Anup.IsRevertingActionOngoing &&
		!Amphora.IsPushedOverTableEdge &&
		!Amphora.IsCracked &&
		!Cat.IsPushAndMoveInProgress &&
		!ScreenFader.IsInProgress &&
		!GameController.IsGameCompleted;

	private bool wasReadyToBegin;

	public bool IsActive { get; private set; }

	private void Awake()
	{
		ActivateReadyHint(false);
		IsActive = false;
	}

	private void Update()
	{
		if (IsReadyToBegin && !wasReadyToBegin)
		{
			ActivateReadyHint(true);
		}
		
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
		{
			if (IsReadyToBegin)
				Activate();
		}
		
		wasReadyToBegin = IsReadyToBegin;
	}

	private void Activate()
	{
		ActivateReadyHint(false);
		
		// TODO: Initiate the raise bar mechanic
		// Deal with user input
		// TODO and then:
		
		cat.Push(Cat.PushPower.Low);
		
		StartCoroutine(DeactivatedReleaseCoroutine());
	}
	IEnumerator DeactivatedReleaseCoroutine()
	{
		yield return new WaitForSeconds(1f);
		IsActive = false;
	}

	private void ActivateReadyHint(bool value)
	{
		hint.SetActive(value);
	}
}
