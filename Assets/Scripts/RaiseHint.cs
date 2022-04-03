using System.Collections;
using UnityEngine;

public class RaiseHint : MonoBehaviour
{
	[SerializeField] private GameObject glyphLMB0;
	[SerializeField] private GameObject glyphLMB1;

	private void Awake()
	{
		SetState(0);
	}

	private void OnEnable()
	{
		StopAllCoroutines();
		StartCoroutine(BlinkCoroutine());
	}

	IEnumerator BlinkCoroutine()
	{
		yield return new WaitForSeconds(5f);
		do
		{
			SetState(1);
			yield return new WaitForSeconds(0.5f);
			SetState(2);
			yield return new WaitForSeconds(1f);
		}
		while (true);
	}

	private void OnDisable()
	{
		StopAllCoroutines();
		SetState(0);
	}

	private void SetState(int state)
	{
		glyphLMB0.SetActive(state == 1);
		glyphLMB1.SetActive(state == 2);
	}
}
