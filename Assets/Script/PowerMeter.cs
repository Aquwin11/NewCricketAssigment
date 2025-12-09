using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PowerMeter : MonoBehaviour
{
    public RectTransform indicatorRect;
    public float minHeight;
    public float maxHeight;
    public float speed;

    public bool isMovingUp = true;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(isMovingUp)
        {
            indicatorRect.anchoredPosition += Vector2.up * speed * Time.deltaTime;
            if (indicatorRect.anchoredPosition.y >= maxHeight)
            {
                isMovingUp = false;
            }
        }
        else
        {
            indicatorRect.anchoredPosition += Vector2.down * speed * Time.deltaTime;
            if (indicatorRect.anchoredPosition.y <= minHeight)
            {
                isMovingUp = true;
            }
        }
    }

    public float GetStrength()
    {
        float yPos = indicatorRect.anchoredPosition.y;
        yPos = Mathf.Abs(yPos);
        //yPos *= -1;
        float value = 110 - yPos;
        value = Mathf.Abs(value);
        return value/110;
    }
}
