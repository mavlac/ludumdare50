using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public class RaiseValue : MonoBehaviour
{
	[SerializeField] private GameObject valueCanvas;
	[SerializeField] private RectTransform value;
	[SerializeField] private GameObject glyphs;

	private Raise parentRaise;

	private bool isShown;

	private void Awake()
	{
		parentRaise = GetComponentInParent<Raise>();
		Assert.IsNotNull(parentRaise);
		
		Show(false);
	}

	private void Update()
	{
		if (parentRaise.IsActive && !parentRaise.IsReleasing && !isShown)
		{
			Show(true);
		}
		else if ((!parentRaise.IsActive || parentRaise.IsReleasing) && isShown)
		{
			Show(false);
		}

		value.localScale = new Vector3(parentRaise.NormalizedPushPower, 1f, 1f);
	}

	private void Show(bool value)
	{
		isShown = value;
		valueCanvas.SetActive(value);
		glyphs.SetActive(value);
	}
}
