using UnityEngine;

public class Rotate : MonoBehaviour
{
	public Space space = Space.World;
	public Vector3 rotationVector = new Vector3(0f, 0f, 10);

	void Update()
	{
		transform.Rotate(rotationVector * Time.deltaTime, space);
	}
}