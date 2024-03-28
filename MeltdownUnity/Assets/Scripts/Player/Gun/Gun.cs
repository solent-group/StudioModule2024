using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Gun script is responsible of controlling the Gun and firing it.
/// </summary>
public class Gun : MonoBehaviour
{
	[Header("Fire and Ice prefab projectiles")]
	[Tooltip("The prefab with the <b>Fire</b> script attached")]
	public GameObject FirePrefab;

	[Tooltip("The prefab with the <b>Ice</b> script attached")]
	public GameObject IcePrefab;

	[Header("Projectile settings")]
	[Tooltip("Where the projectile is held and fired from")]
	public Transform BulletSpawnPoint;

	[Tooltip("The rate at which each projectile is fire")]
	public float FireRate = 0.3f;

	[Header("Conditions for firing the weapon")]
	[Tooltip("Weather the player can fire fire")]
	public bool CanUseFire = true;

	[Tooltip("Weather the player can fire ice")]
	public bool CanUseIce = true;

	// Private variables for use inside the script.
	// For getting the current ammo ammount. (It was like that when I got here).
	private AmmoController AmmoController;

	// For storing the bullet game object so we can do stuff to it.
	private GameObject currentBullet;

	// For delaying the shots fired from the weapon.
	private float _LocalTime;

	// Used for raycasts. Mothing else. [We do this so we dont make C++ Engine calls, it's call caching]
	private Camera cam;

	void Start()
	{
		// Set the referances / variables. We do this so we dont make C++ Engine calls.
		AmmoController = gameObject.GetComponent<AmmoController>();
		cam = Camera.main;
	}

	void Update()
	{
		// If the game is paused, we dont want to be able to shoot the weapon.
		if (PauseMenu.Paused) return;

		// Cache the raycast in a tempoary variable.
		RaycastHit hit;

		// Check to see if the player is looking at a object. 
		// This allows the projectile to hit centre of screen.
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
		{
			// Make the projectile look towards the centre of the screen.
			// Before, the projectile would be off centre.
			BulletSpawnPoint.LookAt(hit.point);
		}
		else
		{
			// Reset the rotation of the projectile as the player is looking at the void.
			BulletSpawnPoint.localRotation = Quaternion.identity;
		}

		// This increments the local time if the localtime is less than the fireate plus one.
		// Used to prevent a long error. If you keep adding to the variable, it will eventually break.
		if (_LocalTime < FireRate + 1f) _LocalTime += Time.deltaTime;

		// If the player is holding the shoot fire button, there is sufficiant ammo, the local time is greater than the firerate and the current bullet is null.
		if (Input.GetKeyDown(InputManager.GetKey(InputActions.KeyAction.ShootFire)) && AmmoController.FireAmmo > 0 && _LocalTime > FireRate && currentBullet == null)
		{
			// Lower the ammo by 1.
			AmmoController.FireAmmo -= 1;

			// Instantiate the bullet GameObject and store it for later. We also parent the object to the spawn point.
			currentBullet = Instantiate(FirePrefab, Vector3.zero, BulletSpawnPoint.rotation, BulletSpawnPoint);

			// Disables the rigidbody to stop a bug where the physics engine also moves the object.
			currentBullet.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
			currentBullet.GetComponent<Rigidbody>().isKinematic = true;

			// Constantly set the position to zero, keeping it with the player.
			currentBullet.transform.localPosition = Vector3.zero;
		}
		// Else if the player lets go of the shoot fire button and the current bullet is null. (Shoot it).
		else if (Input.GetKeyUp(InputManager.GetKey(InputActions.KeyAction.ShootFire)) && currentBullet != null)
		{
			// Reset the time to zero so the plauer will have to wait for the fireate time to shoot again.
			_LocalTime = 0;


			// Check if the bullet GameObject is not null and has the relevant script.
			if (currentBullet != null && currentBullet.GetComponent<Fire>() != null)
			{
				// Get the projectile script and activate the projectile.
				currentBullet.GetComponent<Fire>().Activate();

				// Enable the rigidbody.
				currentBullet.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
				currentBullet.GetComponent<Rigidbody>().isKinematic = false;

				// Apply velocity to the bullet Rigidbody
				currentBullet.GetComponent<Rigidbody>().velocity = BulletSpawnPoint.forward * currentBullet.GetComponent<Fire>().Speed;
			}

			// Set the bullet ref to null, we shoot it.
			currentBullet = null;
		}


		// If the player is holding the shoot ice button, there is sufficiant ammo, the local time is greater than the firerate and the current bullet is null.
		if (Input.GetKeyDown(InputManager.GetKey(InputActions.KeyAction.ShootIce)) && AmmoController.IceAmmo > 0 && _LocalTime > FireRate && currentBullet == null)
		{
			// lower the ammo by 1.
			AmmoController.IceAmmo -= 1;

			// Instantiate the bullet GameObject and store it for later. We also parent the object to the spawn point.
			currentBullet = Instantiate(IcePrefab, Vector3.zero, BulletSpawnPoint.rotation, BulletSpawnPoint);

			// Disables the rigidbody to stop a bug where the physics engine also moves the object.
			currentBullet.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
			currentBullet.GetComponent<Rigidbody>().isKinematic = true;

			// constantly set the position to zero, keeping it with the player.
			currentBullet.transform.localPosition = Vector3.zero;
		}
		// else if the player lets go of the shoot ice button and the current bullet is null. (Shoot it).
		else if (Input.GetKeyUp(InputManager.GetKey(InputActions.KeyAction.ShootIce)) && currentBullet != null)
		{
			// reset the time to zero so the plauer will have to wait for the fireate time to shoot again.
			_LocalTime = 0;


			// Check if the bullet GameObject is not null and has the relevant script.
			if (currentBullet != null && currentBullet.GetComponent<Ice>() != null)
			{
				// Get the projectile script and activate the projectile.
				currentBullet.GetComponent<Ice>().Activate();

				// Enable the rigidbody.
				currentBullet.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
				currentBullet.GetComponent<Rigidbody>().isKinematic = false;

				// Apply velocity to the bullet Rigidbody
				currentBullet.GetComponent<Rigidbody>().velocity = BulletSpawnPoint.forward * currentBullet.GetComponent<Ice>().Speed;
			}

			// Set the bullet ref to null, we shoot it.
			currentBullet = null;
		}
	}
}
