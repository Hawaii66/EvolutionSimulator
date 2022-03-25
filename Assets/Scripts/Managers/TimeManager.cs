using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public bool isPaused;
    public float timeScale;

    void SetTime(float newState, bool changeTimeScale)
    {
        Time.timeScale = newState;
        if (changeTimeScale)
        {
            timeScale = Time.timeScale;
        }
    }

    void ChangeTime(float newState)
    {
        Time.timeScale += newState;
        timeScale = Time.timeScale;
    }

    void Start()
    {
        SetTime(1f, true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetTime(isPaused ? 0 : timeScale, false);
            isPaused = !isPaused;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeTime(-0.5f);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeTime(0.5f);
        }
    }
}
