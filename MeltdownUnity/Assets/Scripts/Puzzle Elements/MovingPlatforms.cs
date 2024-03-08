using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
	public float PlatformSpeed = 1f;

	public bool Activated = false;

	public Transform PlatformObject;
	public Vector3 platformStartPos;
	public Vector3 platformEndPos;

	private float _timeCounter;

	// Update is called once per frame
	void Update()
	{
		// closed
		if (_timeCounter < 1 && Activated)
		{
			_timeCounter += Time.deltaTime * PlatformSpeed;
		}
		// open
		else if (_timeCounter > 0 && !Activated)
		{
			_timeCounter -= Time.deltaTime * PlatformSpeed;
		}

		// move platform.
		PlatformObject.localPosition = Vector3.Lerp(platformStartPos, platformEndPos, _timeCounter);
	}

	public void ToggleDoor()
	{
		Activated = !Activated;
	}

	public void SetActivated(bool activated)
	{
		Activated = activated;
	}
}
