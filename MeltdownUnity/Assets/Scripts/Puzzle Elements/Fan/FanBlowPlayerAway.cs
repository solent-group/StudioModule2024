using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlowPlayerAway : MonoBehaviour
{
    const float BlowMultiplier = 0.3f;

    public GameObject playerObject;
    public PlayerMovementController playerMovementController;
    public FanSpin fanSpin;

    [Header("Suck Player in or Blow Player Away?")] public bool blowingInwards;
    [Header("Apply Player Y Axis Interference?")] public bool yAxisEnabled;

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
            if(yAxisEnabled)
            {
                if(blowingInwards)
                {
                    //suck player inwards
                    playerMovementController.velocity += (transform.position - other.transform.position) * BlowMultiplier;
                }
                else
                {
                    //blow player outwards
                    playerMovementController.velocity += -(transform.position - other.transform.position) * BlowMultiplier;
                }
            }
            else
            {
                if(blowingInwards)
                {
                    //suck player x and z inwards
                    playerMovementController.velocity.x += (transform.position.x - other.transform.position.x) * BlowMultiplier;
                    playerMovementController.velocity.z += (transform.position.z - other.transform.position.z) * BlowMultiplier;
                }
                else
                {
                    //blow player x and z outwards
                    playerMovementController.velocity.x += -(transform.position.x - other.transform.position.x) * BlowMultiplier;
                    playerMovementController.velocity.z += -(transform.position.z - other.transform.position.z) * BlowMultiplier;
                }
            }
        }
    }
}
