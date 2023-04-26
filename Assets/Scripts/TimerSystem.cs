using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerSystem : MonoBehaviour
{
    private bool timerOn = false;

    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float timeLeft;

    private void Start()
    {
        timerOn = true;
    }

    private void Update()
    {
        if (timerOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;

                updateTime(timeLeft);
            }
            else
            {
                Debug.Log("Time is up");
                timeLeft = 0.0f;
                timerOn = false;
            }
        }
    }

    private void updateTime(float currTime)
    {
        currTime += 1f;

        float minutes = Mathf.FloorToInt(currTime / 60f);
        float seconds = Mathf.FloorToInt(currTime % 60f);

        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}