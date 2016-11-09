using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Contains besic movement for any kind of extended ai.
/// Rotates and moves based on smoothed movement - horizontal and vertical
/// Smiliar to player's version of horizontal and vertical.
/// 
/// Allows forward, back movement together with left and right rotation, with instant or slowdown effect idle
/// </summary>
public class SimpleUnit : MonoBehaviour {

    float horizontal;
    float vertical;

    public float acceleration = 1; // how much faster it will go to max speed, cant be 0, 0.5=2x faster
    public float mobility = 1; // how much faster it will go to max rotation, cant be 0, 0.5=2x faster

    public float speed = 2;
    public float steering = 20;

    string movementState = "idle";
    string rotationState = "idle";

    public enum Moves
    {
        forward,
        back,
        left,
        right,
        idle
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(new Vector3(0, vertical) * Time.deltaTime * speed);
        transform.Rotate(new Vector3(0, 0, horizontal)*Time.deltaTime * steering);

	}

    public void Idle(bool instantlyResetMove = false, bool instantlyResetRot = true
        , bool resetMovement = true, bool resetRotation = true)
    {// slowly eases into idle

        // movement reset
        if (resetMovement)
        {
            if (instantlyResetMove)
            {
                vertical = 0;
                movementState = "idle";
            }
            else
            {
                if (movementState != "idle")
                {// these ifs are here because coroutine has to run only once
                    float time = ((vertical + 1) / 2) / acceleration;
                    StartCoroutine(Lerp(SetVertical, vertical, 0, time, null));//normalize so it needs up to 1s
                    movementState = "idle";
                }
            }
        }
        // rotation reset
        if (resetRotation)
        {
            if (instantlyResetRot)
            {
                horizontal = 0;
                rotationState = "idle";
            }
            else
            {
                if (rotationState != "idle")
                {
                    float time = ((horizontal + 1) / 2) / mobility;
                    StartCoroutine(Lerp(SetHorizontal, horizontal, 0, time, null));//normalize so it needs up to 1s
                    rotationState = "idle";
                }
            }
        }
    }

    public void ExecuteMove(Moves moveType,
        bool overridePrevMoves = false, 
        bool stopWhenDone = false, 
        bool resetFirst = false, 
        Action stopLerpCoro = null)
    {
        string keyword = moveType.ToString();
        switch (moveType)
        {
            // movement
            case Moves.forward:
                if (movementState == keyword) return;
                else movementState = keyword;
                Forward(overridePrevMoves, stopWhenDone, resetFirst);
                break;
            case Moves.back:
                if (movementState == keyword) return;
                else movementState = keyword;
                Back(overridePrevMoves, stopWhenDone, resetFirst);
                break;
            // rotation
            case Moves.left:
                if (rotationState == keyword) return;
                else rotationState = keyword;
                TurnLeft(overridePrevMoves, stopWhenDone, resetFirst);
                break;
            case Moves.right:
                if (rotationState == keyword) return;
                else rotationState = keyword;
                TurnRight(overridePrevMoves, stopWhenDone, resetFirst, stopLerpCoro);

                break;
            case Moves.idle:
                Idle();
                break;
            default:
                break;
        }
    }
    
    void Forward(bool overrideRotation = true, bool stopWhenDone = false, bool resetFirst = false, Action stopLerpCoro = null)
    {
        if (overrideRotation)
        {
            horizontal = 0;
        }

        if (resetFirst)
        {
            vertical = 0;
        }
        Debug.Log(vertical+" "+ resetFirst + " "+ vertical / acceleration);
        StopCoroutine("Lerp");
        float time = ((vertical+1)/2)/acceleration;
        StartCoroutine(Lerp(SetVertical, vertical, 1, time, stopLerpCoro));//normalize so it needs up to 1s

    }

    void Back(bool overrideRotation = true, bool stopWhenDone = false, bool resetFirst = false, Action stopLerpCoro = null)
    {
        if (overrideRotation)
        {
            horizontal = 0;
        }

        if (resetFirst)
        {
            vertical = 0;
        }
        StopCoroutine("Lerp");
        float time = ((vertical + 1) / 2) / acceleration;

        StartCoroutine(Lerp(SetVertical, vertical, -1, time, stopLerpCoro));//normalize so it needs up to 1s

    }

    void TurnLeft(bool overrideMovement = true, bool stopWhenDone = false, bool resetFirst = false, Action stopLerpCoro = null) {
        
        if (overrideMovement)
        {
            vertical = 0;
        }
        if (resetFirst)
        {
            horizontal = 0;
        }

        StopCoroutine("Lerp");
        float time = ((horizontal + 1) / 2) / mobility;
        StartCoroutine(Lerp(SetHorizontal, horizontal, 1, time, stopLerpCoro));//normalize so it needs up to 1s
    }

    void TurnRight(bool overrideMovement = true, bool stopWhenDone = false, bool resetFirst = false, Action stopLerpCoro = null)
    {
        if (overrideMovement)
        {
            vertical = 0;
        }
        if (resetFirst)
        {
            horizontal = 0;
        }

        StopCoroutine("Lerp");
        float time = ((horizontal + 1) / 2) / mobility;
        StartCoroutine(Lerp(SetHorizontal, horizontal, -1, time, stopLerpCoro));//normalize so it needs up to 1s

    }

    void SetHorizontal(float value)
    {
        horizontal = value;
    }

    void SetVertical(float value)
    {
        vertical = value;
    }

    private IEnumerator Lerp(Action<float> callback, float startValue, float endValue, float time, Action onDone)
    {
        float i = 0.0f;
        float rate = 1.0f / time;
        while (i < 1.0)
        {
            i += Time.deltaTime * rate;
            callback(Mathf.Lerp(startValue, endValue, i));
            
            yield return null;
        }
        if (onDone != null)
        {
            onDone();
        }
    }
}
