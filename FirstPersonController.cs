using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour {

    CharacterController cc;
    
    public float speedMultiplier = 1;
    public float mouseSensitivity = 3;// Mouse sensitivity
    public float upDownRange = 60f;
    public float jumpSpeed = 10f;

    float verticalRotation;

    float verticalVelocity = 0;

	// Use this for initialization
	void Start () {
        

        cc = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {

        PlayerMovRot(transform, cc, mouseSensitivity, upDownRange, ref verticalRotation, speedMultiplier);
	}

    protected virtual void PlayerMovRot(Transform transform, CharacterController cc, float mouseSensitivity, float upDownRange, ref float verticalRotation, float speedMultiplier)
    {
        // Rotation
        LeftRightRotation(mouseSensitivity);

        verticalRotation = VerticalRotation(mouseSensitivity, upDownRange, verticalRotation);

        // Movement
        Movement(transform, cc, speedMultiplier);
    }

    protected virtual void Movement(Transform transform, CharacterController cc, float speedMultiplier)
    {
        float forward = Input.GetAxis("Vertical") * speedMultiplier;
        float right = Input.GetAxis("Horizontal") * speedMultiplier;

        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        verticalVelocity = Jump(cc, verticalVelocity);

        Vector3 speed = new Vector3(right, verticalVelocity, forward);

        // Modify movement by rotation
        speed = transform.rotation * speed;

        cc.Move(speed * Time.deltaTime);
    }

    protected virtual float Jump(CharacterController cc, float verticalVelocity)
    {
        if (cc.isGrounded && Input.GetButtonDown("Jump"))
        {
            return verticalVelocity = jumpSpeed;
        }
        return verticalVelocity;
    }

    /// <summary>
    /// ADD, +=, return value to vertical velocity.
    /// </summary>
    /// <param name="mouseSensitivity"></param>
    /// <param name="upDownRange"></param>
    /// <param name="verticalRotation"></param>
    /// <returns></returns>
    protected virtual float VerticalRotation(float mouseSensitivity, float upDownRange, float verticalRotation)
    {
        verticalRotation = verticalRotation - Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);// Euler angler can go 0>360 at 0

        return verticalRotation;
    }

    protected virtual void LeftRightRotation(float mouseSensitivity)
    {
        float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, rotLeftRight, 0);
    }
}
