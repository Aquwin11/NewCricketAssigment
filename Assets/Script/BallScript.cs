using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public Transform startPos;
    public Transform pitchPos;
    public Rigidbody ballRB;
    public float ballSpeed;
    public bool isThrown;
    public float arcHeight = 5f; // Controls the height of the arc
    public float duration; // Depends based on the start and end
    public LayerMask pitchLayer;
    public float timer;
    public Vector3 ballDirection;
    [Range(-1,1)]
    public float leftRightSlider =0;
    public float pitchCurve;
    public float pitchCurveMultiplier;
    public float pitchBounceValue;
    public bool isSwing;
    public float  curveDuration;
    public float curveAmount;
    public bool isRight;
    public void OnEnable()
    {
        /*transform.position = startPos.position;
        duration = Vector3.Distance(pitchPos.position, startPos.position);
        ballDirection = CalculateDir();
        Debug.Log("Distance " + duration);*/
        Destroy(gameObject,4f);
    }
    public void SetupBallScript(Transform pitchT, float slideLR, float strength)
    {
        pitchPos = pitchT;
        leftRightSlider = slideLR;
        pitchCurveMultiplier = strength;
        Debug.Log("sET bALL " + duration);


        transform.position = startPos.position;
        duration = Vector3.Distance(pitchPos.position, startPos.position);
        ballDirection = CalculateDir();
        Debug.Log("Distance " + duration);


    }

    public void Update()
    {
        if(!isThrown)
        {/*
            timer += Time.deltaTime;
            float t = timer / duration;
            float xOffset = arcHeight * (4 * t - 4 * t);*/
            //ballDirection.x -= xOffset;
            //* ballSpeed * Time.deltaTime
        }

    }
    float curveX=0;
    public void FixedUpdate()
    {
        RaycastHit hit; 
        if (!isThrown)
        {
            
            /*if(isSwing)
            {
                timer += Time.fixedDeltaTime;

                Vector3 forwardVel = ballDirection * ballSpeed;

                float curveX = -Mathf.Sin(timer * 2) * curveAmount;
                

                Vector3 curve = new Vector3(curveX, 0, 0);

                ballRB.velocity = forwardVel + curve + new Vector3(-curveAmount + curveAmount/3, 0,0);
            }*/
            if (isSwing)
            {
                if(isRight)
                {
                    timer += Time.fixedDeltaTime;

                    Vector3 forwardVel = ballDirection * ballSpeed;

                    float curveX = -Mathf.Sin(timer * 2) * curveAmount;


                    Vector3 curve = new Vector3(curveX, 0, 0);

                    ballRB.velocity = forwardVel + curve + new Vector3(-curveAmount + curveAmount * 1.65f, 0, 0);
                }
                else
                {
                    timer += Time.fixedDeltaTime;

                    Vector3 forwardVel = ballDirection * ballSpeed;

                    float curveX = Mathf.Sin(timer * 2) * curveAmount;


                    Vector3 curve = new Vector3(curveX, 0, 0);

                    ballRB.velocity = forwardVel + curve + new Vector3(-curveAmount + curveAmount / 3, 0, 0);
                }
            }
            else
            {
                ballRB.velocity = ballDirection * ballSpeed;

            }
        }
        else
        {
            if(Physics.Raycast(transform.position, Vector3.forward, out hit, 2f, pitchLayer))
            {
                contactNormalize = hit.point;
            }
        }
    }
    public Vector3 CalculateDir()
    {
        Vector3 dir = pitchPos.position - startPos.position;
        dir.Normalize();
        return dir;
    }

    Vector3 contactNormalize;
    Vector3 contactPointPos;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Pitch")
        {
            Debug.Log("Hit floor");
            isThrown = true;
            /*ballRB.isKinematic = true;
            ballRB.useGravity = false;*/
            ContactPoint contactPoint = collision.contacts[0];
            contactPointPos = contactPoint.point;
            contactNormalize = contactPoint.normal;
            Vector3 reflectedVelocity = Vector3.Reflect(ballRB.velocity, contactNormalize);
            ballRB.velocity = Vector3.zero;
            pitchCurve = pitchCurve * pitchCurveMultiplier;

            // Apply the reflected velocity with a bounce strength multiplier
            ballRB.velocity = reflectedVelocity * 0.15f;
            //ballRB.velocity.x += pitchCurve * leftRightSlider;
            Vector3 ballNewVelocity = new Vector3(ballRB.velocity.x + (pitchCurve * leftRightSlider),
                ballRB.velocity.y,
                ballRB.velocity.z);
            pitchBounceValue = (30 - duration)/3f ;
            Debug.Log("Pitch Bounce" + pitchBounceValue);
            //pit
            ballRB.AddForce(ballNewVelocity / 0.15f + (Vector3.up * pitchBounceValue),ForceMode.Impulse);
        }
        else if(collision.gameObject.tag =="Wicket")
        {
            Debug.Log("OUT");
        }
    }

    public void OnDrawGizmos()
    {
        if (isThrown)
        {
            Gizmos.DrawLine(contactPointPos, contactPointPos + contactNormalize);
        }
    }

    public void SetStartPos(Transform newPos)
    {
        startPos = newPos;
    }
}
