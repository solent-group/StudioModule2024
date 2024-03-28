
using UnityEngine;

//How the code works
//If there isn't one already create a empty gameobject and call it "HoldArea"
//Attach the created gameobject to mainCamera parent
//Place the Hold Area gameobject away from the camera but still close to the player (X = 0, Y = 0.5, Z = between 2 and 3)
//Make sure the mainCamera has the PickupController code attacked to the camera
//Assign the HoldArea gameobject to the Hold Area Variable on the script component
//Make sure the object you are having the player pick up has a Rigitbody component
//Make sure the object you are trying to pick up has the tag "MoveableObject"
//Make sure the gun object is attached to the variable on the script component
//Make sure the player is attached to the player variable on the script component
public class PickupController : MonoBehaviour
{

    [Header("Pickup Settings")]
    [SerializeField] Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;

    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;
    [SerializeField] private float playerDistance = 1.0f;
    [SerializeField] private int minMassObject = 2;

    public GameObject mainGun;

    Gun gun;
    public GameObject player;

    void Awake()
    {
        //Links the gun script tied to the player object to this script.
        gun = player.GetComponent<Gun>();
    }

    private void Update()
    {
        //Checks all of the time when the player is going to pick something up.
        //The Input is set to a toggle.
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Checks if at any point the player is actually holding an item.
            if (heldObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    //Only picks up object with the tag "MoveableObject"
                    if (hit.transform.CompareTag("MoveableObject"))
                    {
                        PickupObject(hit.transform.gameObject);
                        //Call Audio Manager (Player)
                    }
                }
            }
            else
            {
                DropObject();
            }
        }
        //Check if the player actually has an object at any time.
        if (heldObj != null)
        {
            MoveObject();
        }

        void MoveObject()
        {
            //Checking the distance between the held object and then the hold area to make sure thats it's greater then 0.1.
            if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
            {
                //Where it's going to move to.
                Vector3 moveDirection = (holdArea.position - heldObj.transform.position);
                heldObjRB.AddForce(moveDirection * pickupForce);

                //Drops the object if its get to far from the player
                if (Vector3.Distance(heldObj.transform.position, holdArea.position) > playerDistance)
                {
                    DropObject();
                }
            }
        }

        void PickupObject(GameObject pickObj)
        {
            if (pickObj.GetComponent<Rigidbody>())
            {
                heldObjRB = pickObj.GetComponent<Rigidbody>();

                //Only picks up the object if its lower then the minMassObject
                if (heldObjRB.mass < minMassObject)
                {
                    //Disables the gravity on the object when its been picked up.
                    heldObjRB.useGravity = false;

                    heldObjRB.drag = 10;

                    //When object is picked up it's not going to rotate around.
                    heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;

                    heldObjRB.isKinematic = true;

                    //Parents the object to the hold area.
                    heldObjRB.transform.parent = holdArea;
                    heldObj = pickObj;

                    heldObjRB.transform.rotation = Quaternion.Euler(0, 0, 0);

                    //Disables the mainGun as well as allowing the player to fire their ice and/or fire when the object is picked up.
                    mainGun.SetActive(false);
                    gun.CanUseFire = false;
                    gun.CanUseIce = false;
                }
            }
            else
            {
                DropObject();
            }
        }

        void DropObject()
        {
            //Enables the gravity on the object when its been dropped.
            heldObjRB.useGravity = true;

            heldObjRB.drag = 1;

            //When the object is dropped it can rotate again.
            heldObjRB.constraints = RigidbodyConstraints.None;

            //Unparents the object from the hold area.
            heldObj.transform.parent = null;
            heldObj = null;

            heldObjRB.isKinematic = false;

            //Enables the mainGun as well as allowing the player to fire their ice and/or fire when the object is dropped'
            mainGun.SetActive(true);
            gun.CanUseFire = true;
            gun.CanUseIce = true;

            //Add a impact check to allow calling the audio manager
        }
    }
}
