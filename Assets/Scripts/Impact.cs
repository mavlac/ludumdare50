using UnityEngine;
using System.Collections;

public class Impact : MonoBehaviour
{
	[SerializeField] private GameObject graphic;

	[Header("Scene Objects")]
	[SerializeField] private Amphora amphora;

	private void Awake()
	{
		graphic.SetActive(false);

		amphora.Cracked += OnAmphoraCracked;
	}

	private void OnDestroy()
	{
		amphora.Cracked -= OnAmphoraCracked;
	}

	private void OnAmphoraCracked()
	{
		transform.position = new Vector3(amphora.CenterPosition.x, transform.position.y, transform.position.z);

		StartCoroutine(ImpactLifetimeCoroutine());
	}

	IEnumerator ImpactLifetimeCoroutine()
	{
		graphic.SetActive(true);
		yield return new WaitForSeconds(0.15f);
		graphic.SetActive(false);
	}
}
