using System;
using System.Collections.Generic;
using UnityEngine;

public class Amphora : MonoBehaviour
{
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip crackAudioClip;

	[Space]
	[SerializeField] private GameObject defaultShape;
	[SerializeField] private List<GameObject> brokenPieces;

	[Space]
	[SerializeField] private ParticleSystem crackParticleSystem;

	bool isCracked = false;

	public event Action Cracked;

	private void Awake()
	{
		brokenPieces.ForEach((brokenPiece) => brokenPiece.SetActive(false));
	}

	private void Update()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.C))
			Crack();
#endif
	}

	public void Crack()
	{
		if (isCracked)
			return;

		isCracked = true;

		defaultShape.SetActive(false);
		crackParticleSystem.Play();
		brokenPieces.ForEach((brokenPiece) => brokenPiece.SetActive(true));

		crackParticleSystem.Play();

		audioSource.PlayOneShot(crackAudioClip);

		Cracked?.Invoke();
	}
}
