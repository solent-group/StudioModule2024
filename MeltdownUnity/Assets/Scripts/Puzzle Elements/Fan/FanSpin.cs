using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanSpin : MonoBehaviour
{
    const float FastSpinSpeed = 1;
    const float SlowSpinSpeed = 0.05f;

    public float SlowTime;
    public bool isSlow;

    [SerializeField] private float CurrentSpinSpeed;

    public bool isClockwise = false;

    // Start is called before the first frame update
    void Start()
    {
        //set initial values
        CurrentSpinSpeed = FastSpinSpeed;
    }

    private void Update()
    {
        if(isClockwise)
        {
            //rotate clockwise
            transform.Rotate(Vector3.down * CurrentSpinSpeed);
        }
        else
        {
            //rotate anticlockwise
            transform.Rotate(Vector3.up * CurrentSpinSpeed);
        }
    }

    IEnumerator SlowFan()
    {
        //change spin speed to slower for slowtime number of seconds
        isSlow = true;
        CurrentSpinSpeed = SlowSpinSpeed;
        yield return new WaitForSeconds(SlowTime);
        CurrentSpinSpeed = FastSpinSpeed;
        isSlow = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if collide with ice then slow fan
        if (collision.gameObject.GetComponent<Ice>() != null)
        {
            StartCoroutine("SlowFan");
        }
    }
}
