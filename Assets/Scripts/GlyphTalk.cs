using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlyphTalk : MonoBehaviour
{
	const float GlyphDuration = 0.5f;

	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private List<Sprite> sprites;

	private void Awake()
	{
		spriteRenderer.enabled = false;
	}

	public void RandomTalk(int glyphs, float preDelay = 0f)
	{
		spriteRenderer.enabled = false;
		StopAllCoroutines();
		StartCoroutine(RandomTalkCoroutine(glyphs, preDelay));
	}
	IEnumerator RandomTalkCoroutine(int glyphs, float preDelay)
	{
		yield return new WaitForSeconds(preDelay);
		
		spriteRenderer.enabled = true;
		
		do
		{
			spriteRenderer.sprite = sprites.Random();
			glyphs--;
			yield return new WaitForSeconds(GlyphDuration);
		}
		while (glyphs > 0);

		spriteRenderer.enabled = false;
	}
}
