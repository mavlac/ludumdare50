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

	[Header("Scene Objects")]
	[SerializeField] private Transform tableEdgePivot;
	[SerializeField] private Cat cat;

	private Vector3 initialPosition;
	bool isCracked = false;

	public static bool IsPushedOverTableEdge { get; private set; }
	public Vector3 CenterPosition => centerPivot.position;

	public event Action Cracked;


	private void Awake()
	{
		brokenPieces.ForEach((brokenPiece) => brokenPiece.SetActive(false));
		initialPosition = transform.position;
		IsPushedOverTableEdge = false;

		defaultRigidbody.freezeRotation = true;
	}

	private void Start()
	{
		cat.PushPerformed += OnCatPushed;
	}

	private void OnDestroy()
	{
		cat.PushPerformed -= OnCatPushed;
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
		defaultRigidbody.freezeRotation = true;
		transform.DOMove(initialPosition, duration).SetEase(Ease.InOutCubic);
		transform.DORotate(Vector3.zero, duration).SetEase(Ease.InOutCubic);
	}

	private void OnCatPushed(Cat.PushPower pushPower)
	{
		if (Anup.IsRevertingActionOngoing)
			return;

		switch (pushPower)
		{
			case Cat.PushPower.Low:
				Klink();
				break;

			case Cat.PushPower.Ideal:
				Klink();
				Move(defaultRigidbody.position + Vector2.left * Cat.PushIdealDistance);
				break;

			case Cat.PushPower.Harsh:
				HarshPush(); // TODO: check if not pushed harshly over table edge - in this case it's win
				// TODO: also implement no magic in Anup
				break;
		}
	}

	private void Klink()
	{
		defaultShape.transform.DOPunchRotation(Vector3.forward * 5f, 0.5f, vibrato: 3, elasticity: 1).OnComplete(() =>
		{
			defaultShape.transform.localRotation = Quaternion.identity;
		});
	}

	private void Move(Vector2 position)
	{
		defaultRigidbody.MovePosition(position);

		if (position.x < tableEdgePivot.position.x)
		{
			defaultRigidbody.freezeRotation = false;
			IsPushedOverTableEdge = true;
		}
	}

	private void HarshPush()
	{
		// TODO: Add terrible force to flip and wake up Anup
		defaultRigidbody.freezeRotation = false;
	}

	private void Crack()
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
