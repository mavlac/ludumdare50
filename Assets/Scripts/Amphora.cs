using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Amphora : MonoBehaviour
{
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip crackAudioClip;

	[Space]
	[SerializeField] private Transform centerPivot;
	[SerializeField] private Rigidbody2D defaultRigidbody;
	[SerializeField] private Collider2D defaultCollider;
	[SerializeField] private GameObject defaultShape;
	[SerializeField] private List<GameObject> brokenPieces;

	[Space]
	[SerializeField] private ParticleSystem crackParticleSystem;

	private Vector3 initialPosition;
	bool isCracked = false;

	public event Action Cracked;

	public Vector3 CenterPosition => centerPivot.position;


	private void Awake()
	{
		brokenPieces.ForEach((brokenPiece) => brokenPiece.SetActive(false));
		initialPosition = transform.position;
	}

	private void Update()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.C))
			Crack();
#endif
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Floor"))
		{
			Crack();
		}
	}

	public void ReturnToInitialPosition(float duration)
	{
		transform.DOMove(initialPosition, duration).SetEase(Ease.InOutCubic);
	}

	public void Push(float distance)
	{
		if (Anup.IsRevertingActionOngoing)
			return;

		defaultRigidbody.MovePosition(defaultRigidbody.position + Vector2.left * distance);
	}

	public void Crack()
	{
		if (isCracked)
			return;

		isCracked = true;

		defaultShape.SetActive(false);
		defaultRigidbody.isKinematic = true;
		defaultRigidbody.simulated = false;

		crackParticleSystem.Play();
		brokenPieces.ForEach((brokenPiece) =>
		{
			brokenPiece.SetActive(true);
			var force = (brokenPiece.transform.position - centerPivot.position).normalized * 2f;
			brokenPiece.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
		});

		crackParticleSystem.Play();

		audioSource.PlayOneShot(crackAudioClip);

		Cracked?.Invoke();
	}
}
