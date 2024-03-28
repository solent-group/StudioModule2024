using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// This class allows the player to leave in the elevator. call TriggerLeave() to load the next
/// scene which is SceneBuildIndex.
/// </summary>
public class ElevatorController : MonoBehaviour
{
	[Header("Weather the player can use this elevator to leave")]
	public bool IsEnabled = true;

	[Header("What scene will be loaded")]

	[Tooltip("What scene will be loaded")]
	public int SceneBuildIndex = 1;

	[Header("Left Door")]
	public Transform LeftElevatorDoor;
	public Vector3 LeftDoorClosedPos;
	public Vector3 LeftDoorOpenPos;

	[Header("Left Door")]
	public Transform RightElevatorDoor;
	public Vector3 RightDoorClosedPos;
	public Vector3 RightDoorOpenPos;

	[Header("Vars")]

	[Tooltip("How long the wait after the button has been pressed before the next scene is loaded")]
	public float WaitTime = 1.2f;

	[Tooltip("Weather the doors are closed or open")]
	public bool Closed = false;

	[Tooltip("How fast the doors should close and open")]
	public float DoorSpeed = 0.3f;

	// for use in the lerp to lerp the door.
	private float _timeCounter;

	// whehter teh player is in the elevator or not.
	private bool _playerEntered = false;

	private new ParticleSystem particleSystem;

	// Start is called before the first frame update
	void Start()
	{
		//_elevatorStartPos = transform.position;
		particleSystem = GetComponentInChildren<ParticleSystem>();
	}

	// Update is called once per frame
	void Update()
	{
		// closed
		if (_timeCounter < 1 && Closed)
		{
			_timeCounter += Time.deltaTime * DoorSpeed;
		}
		// open
		else if (_timeCounter > 0 && !Closed)
		{
			_timeCounter -= Time.deltaTime * DoorSpeed;
		}

		// move doors.
		LeftElevatorDoor.localPosition = Vector3.Lerp(LeftDoorOpenPos, LeftDoorClosedPos, _timeCounter);
		RightElevatorDoor.localPosition = Vector3.Lerp(RightDoorOpenPos, RightDoorClosedPos, _timeCounter);
	}

	// The void means that it reaturns nothing.
	// On Trigger Enter activates when the attached trigger has another object with a collider enters the trigger.
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (!IsEnabled) return;
			_playerEntered = true;
			SaveData.Current.CurrentLevel = SceneBuildIndex;
			if (SaveManager.current != null) SaveManager.current.ForceSave();
			TriggerLeave();
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			_playerEntered = false;
		}
	}

	/// <summary>
	/// When called the doors will close and this will then start the
	/// Leave coroutine.
	/// </summary>
	public void TriggerLeave()
	{
		if (!IsEnabled) return;
		// redunded now.
		if (!_playerEntered) return;

		if (Random.Range(0, 101) <= 10) particleSystem.Play();

		Closed = true;

		// rise elevator - ienumerator here
		StartCoroutine(Leave());
	}

	/// <summary>
	/// Use to change door state without loading the next scene.
	/// </summary>
	public void CloseOpenToggle()
	{
		Closed = !Closed;
	}

	/// <summary>
	/// This will wait a set ammount of time before loading the next scene.
	/// </summary>
	/// <returns></returns>
	IEnumerator Leave()
	{
		yield return new WaitForSeconds(WaitTime);

		if (LevelLoading.Instance != null) LevelLoading.Instance.LoadScene(SceneBuildIndex);
		else SceneManager.LoadScene(SceneBuildIndex, LoadSceneMode.Single);
	}
}
