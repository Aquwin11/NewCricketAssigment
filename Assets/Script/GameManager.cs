using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public BallScript BallScript;
    public PowerMeter PowerMeter;

    public Transform CurrentSide;
    public Transform leftSide;
    public Transform rightSide;

    public Transform topSide;

    public GameObject Ball;
    [Range(-1, 1)]
    public float leftRightSlider = 0;
    public Transform pitchTransform;

    public bool isSwing;
    public bool isRight;
    public Slider sliderBar;
    public Toggle toggle;
    public void Start()
    {
        CurrentSide = leftSide;
    }

    public void PlayBall()
    {
        Debug.Log("BallStrength" + PowerMeter.GetStrength());
        CreateBall();   
        //BallScript.SetupBallScript(pitchTransform, leftRightSlider, PowerMeter.GetStrength());
        /*BallScript.pitchCurveMultiplier = PowerMeter.GetStrength();
        BallScript.gameObject.SetActive(true);*/
    }


    public void CreateBall()
    {
        GameObject ball = Instantiate(Ball);
        BallScript ballS = ball.GetComponent<BallScript>();
        ball.transform.position = CurrentSide.position;
        ballS.isRight = isRight;
        
        ballS.isSwing = this.isSwing;
        ballS.SetupBallScript(pitchTransform, leftRightSlider, PowerMeter.GetStrength());
    }
    public void SideChange()
    {
        if (CurrentSide == leftSide)
        {
            CurrentSide = rightSide;
            //isRight = true;
        }
        else
        {
            CurrentSide = leftSide;
            //isRight = false;
        }
        BallScript.SetStartPos(CurrentSide);
    }
    public void setIsSwing(bool status)
    {
        isSwing = status; 
    }
    public void sliderChange()
    {
        leftRightSlider = sliderBar.value;
    }
    public void ToggleisRight()
    {
        isRight = toggle.isOn;
    }
    public void Update()
    {
        
    }
}
