using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlowPlayerAway : MonoBehaviour
{
    const float BlowMultiplier = 0.3f;

    public GameObject playerObject;
    public PlayerMovementController playerMovementController;
    public FanSpin fanSpin;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerMovementController = playerObject.GetComponent<PlayerMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
            //Debug.Log(transform.position - playerObject.transform.position);
            //playerMovementController.velocity += -(transform.position - playerObject.transform.position);
    }

    private void OnTriggerStay(Collider other)
    {
        //if not slow then blow the player away
        if (other.CompareTag("Player") && !fanSpin.isSlow)
        {
            playerMovementController.velocity.x += -(transform.position.x - other.transform.position.x) * BlowMultiplier;
            playerMovementController.velocity.z += -(transform.position.z - other.transform.position.z) * BlowMultiplier;
        }
    }
}
