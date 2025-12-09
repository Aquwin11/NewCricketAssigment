using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchController : MonoBehaviour
{
    public Vector2 inputValue;
    public GameManager manager;
    Vector3 newDir;
    public float speed;

    float xClamp;
    float yClamp;
    public void Update()
    {
        inputValue.x = Input.GetAxisRaw("Horizontal");
        inputValue.y = Input.GetAxisRaw("Vertical");

        inputValue.Normalize();


        if(inputValue != Vector2.zero)
        {
            newDir = new Vector3(inputValue.x, 0f, inputValue.y).normalized;
            transform.position += newDir * speed * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Return))
        {
            manager.PlayBall();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            manager.SideChange();
        }
    }

    public void HandleClamping()
    {
        
    }

}
