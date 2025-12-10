using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Vector3 swingPitchOffset;
    public float  curveDuration;
    public float curveAmount;
    public bool isRight;
    public float curveCorrection;
    public void OnEnable()
    {
        /*transform.position = startPos.position;
        duration = Vector3.Distance(pitchPos.position, startPos.position);
        ballDirection = CalculateDir();
        Debug.Log("Distance " + duration);*/
        Destroy(gameObject,4f);
    }
    float newStearingDir = 0;
    //float swingPitchOffset = 0;
    public void SetupBallScript(Transform pitchT, float slideLR, float strength)
    {
        pitchPos = pitchT;
        leftRightSlider = slideLR;
        pitchCurveMultiplier = strength;

        transform.position = startPos.position;

        // Distance and base direction
        duration = Vector3.Distance(pitchPos.position, startPos.position);

        // Approx travel time based on speed (for swing timing)
        // (distance / speed = time)
        curveDuration = duration / ballSpeed;

        ballDirection = CalculateDir();
        timer = 0f;
        if (isRight)
            swingPitchOffset.x = -0.65f;
        else
            swingPitchOffset.x = 0.65f;


        if (leftRightSlider > 0.34f)
            newStearingDir = 1;
        else if (leftRightSlider < -0.34)
            newStearingDir = -1;
        else
            newStearingDir = 0;

        Debug.Log("Stearing dir" + newStearingDir);
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
            if (isSwing)
            {
                // Progress of the ball's flight (0 → 1)
                timer += Time.fixedDeltaTime;
                float t = Mathf.Clamp01(timer / curveDuration);

                // Always aim the forward velocity toward the pitch
                Vector3 toPitch = (pitchPos.position + swingPitchOffset) - transform.position;
                Vector3 forwardDir = toPitch.normalized;
                Vector3 forwardVel = forwardDir * ballSpeed;

                // Sideways direction (perpendicular to forward, on XZ plane)
                Vector3 sideDir = Vector3.Cross(Vector3.up, forwardDir).normalized;

                // Swing curve: starts at 0, peaks mid-air, back to 0 at landing
                // sin(π * t) -> 0 at t=0 and t=1
                float swingStrength = Mathf.Sin(Mathf.PI * t) * curveAmount;

                // Direction: right or left
                if (!isRight)
                    swingStrength = -swingStrength;

                Vector3 swingVel = sideDir * swingStrength;

                // Final velocity
                ballRB.velocity = forwardVel + swingVel;
            }
            else
            {
                // No swing: simple straight delivery
                ballRB.velocity = ballDirection * ballSpeed;
            }
        }
    }

    Vector3 pitchPosition;
    public Vector3 CalculateDir()
    {
        if(isSwing)
            pitchPosition = pitchPos.position + swingPitchOffset;
        else
            pitchPosition = pitchPos.position +( Vector3.forward* 2);
        Vector3 dir = pitchPosition - startPos.position;
        dir.Normalize();
        return dir;
    }

    Vector3 contactNormalize;
    Vector3 contactPointPos;
    float newPitchCurve;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Pitch" && !isThrown)
        {
            Debug.Log("Hit floor");
            isThrown = true;
            ContactPoint contactPoint = collision.contacts[0];
            contactPointPos = contactPoint.point;
            contactNormalize = contactPoint.normal;
            Vector3 reflectedVelocity = Vector3.Reflect(ballRB.velocity, contactNormalize);
            newPitchCurve = (pitchCurve * pitchCurveMultiplier) * newStearingDir;
            pitchBounceValue = (30 - duration) / 4f;//Calculating bounce based on the distance
            pitchBounceValue = Mathf.Clamp(pitchBounceValue, 2.5f, 3.75f);
            Vector3 ballNewVelocity = new Vector3(reflectedVelocity.x,
                reflectedVelocity.y + pitchBounceValue,
                reflectedVelocity.z * 0.75f);
            if(isSwing)//Swing ball pitch action
            {
                ballRB.velocity = Vector3.zero;
                ballNewVelocity = new Vector3((pitchPos.position.x) + swingPitchOffset.x * 10,
                    pitchBounceValue * 1.5f ,
                    transform.forward.z * curveCorrection);
                ballNewVelocity.Normalize();
                ballNewVelocity *= 20;
            }
            //Debug.Log("Reflect Velocity " + ballNewVelocity);
            //Debug.Log("Pitch Bounce" + pitchBounceValue);
            
            ballRB.AddForce(ballNewVelocity + new Vector3(newPitchCurve,0,0), ForceMode.Impulse);//Adding Pitch curve
        }
        else if(collision.gameObject.tag =="Wicket")
        {
            Debug.Log("OUT");
            ballRB.velocity = Vector3.zero;

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
