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

    private float rotateToHorizontal = 0;
    private float moveToVertical = 0;

    public enum Moves
    {
        forward,
        back,
        left,
        right,
        idle
    }
    
    void Start()
    {
        StartCoroutine(RotationLerp());
        StartCoroutine(MovementLerp());
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
            }
            moveToVertical = 0;
            movementState = "idle";
        }
        // rotation reset
        if (resetRotation)
        {
            if (instantlyResetRot)
            {
                horizontal = 0;
            }
            rotateToHorizontal = 0;
            rotationState = "idle";
        }
    }

    public void ExecuteMove(Moves moveType,
        bool overridePrevMoves = false, 
        bool stopWhenDone = false, 
        bool resetFirst = false 
        )
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
                TurnRight(overridePrevMoves, stopWhenDone, resetFirst);

                break;
            case Moves.idle:
                Debug.Log("simple unit idle");
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

        moveToVertical = 1;
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

        moveToVertical = -1;
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

        rotateToHorizontal = 1;
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

        rotateToHorizontal = -1 ;
    }

    /// <summary>
    /// rotation loop that takes care of lerping of horizontal value
    /// 
    /// change 'rotateToHorizontal' to change lerp target.
    /// </summary>
    /// <param name="rotateToHorizontal"></param>
    /// <returns></returns>
    IEnumerator RotationLerp(float rotateToHoriz = 0)
    {
        rotateToHorizontal = rotateToHoriz;
        while (true)
        {
            float rtime = ((horizontal + 1) / 2) / mobility;

            float i = 0.0f;
            float rate = 1.0f / rtime;
            float startValue = horizontal;
            float lastRotationTo = rotateToHorizontal;
            while (i < 1.0)
            {
                if (lastRotationTo != rotateToHorizontal)// make sure calculations are up to date, reset them
                    break;// reset calculations if target lerp changes

                i += Time.deltaTime * rate;
                horizontal = Mathf.Lerp(startValue, rotateToHorizontal, i);
                yield return null;
            }
        }
    }


    IEnumerator MovementLerp(float moveToVert = 0)
    {
        moveToVertical = moveToVert;
        while (true)
        {
            float vtime = ((vertical + 1) / 2) / acceleration;

            float i = 0.0f;
            float rate = 1.0f / vtime;
            float startValue = vertical;
            float lastMoveTo = moveToVertical;
            while (i < 1.0)
            {
                if (lastMoveTo != moveToVertical)
                    break;

                i += Time.deltaTime * rate;
                vertical = Mathf.Lerp(startValue, moveToVertical, i);

                yield return null;
            }
        }
    }
}
