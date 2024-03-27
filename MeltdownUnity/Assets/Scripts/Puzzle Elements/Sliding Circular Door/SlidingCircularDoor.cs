using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingCircularDoor : MonoBehaviour
{
    // [Non-Editor Variables]

    private Transform _doorLeft, _doorRight;

    private Vector3 _defaultLeftPosition, _defaultRightPosition;

    // [Editor Variables]

    [Header("<b>Door Parameters</b>")]
    [Space]
    [Tooltip("The distance the door opens.")][SerializeField][Range(0.0f, 10.0f)] private float _openDistance = 0.6f;
    [Tooltip("The speed the door opens.")][SerializeField][Range(0.0f, 10.0f)] private float _doorSpeed = 0.7f;
    [Tooltip("Whether the door is open.")] public bool openDoor = false;

    // [Events]

    private void Awake()
    {
        _doorLeft = transform.Find("Left Sliding Door");
        _doorRight = transform.Find("Right Sliding Door");
        _defaultLeftPosition = _doorLeft.position;
        _defaultRightPosition = _doorRight.position;
    }

    private void Update()
    {
        if (openDoor)
        {
            MoveDoor(_doorLeft, _defaultLeftPosition - _doorLeft.forward * _openDistance);
            MoveDoor(_doorRight, _defaultRightPosition + _doorRight.forward * _openDistance);

        }
        else
        {
            MoveDoor(_doorLeft, _defaultLeftPosition);
            MoveDoor(_doorRight, _defaultRightPosition);
        }
    }

    // [Custom Methods]

    private void MoveDoor(Transform door, Vector3 targetPosition)
    {
        door.position = Vector3.MoveTowards(door.position, targetPosition, _doorSpeed * Time.deltaTime);
    }
}
