using UnityEngine;
using System.Collections;

public class LockCursor : MonoBehaviour {

	public CursorLockMode wantedMode;

    public bool drawAim = false;
    /// <summary>
    /// looks at point where target target is aiming
    /// </summary>
    public Transform aimRotationTarget;

    void Start()
    {
        SetCursorState();
    }

	// Apply requested cursor state
	void SetCursorState ()
	{
		Cursor.lockState = wantedMode;
		// Hide cursor when locking
		Cursor.visible = (CursorLockMode.Locked != wantedMode);
	}

    void Update()
    {
        if (drawAim)
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
            if (hit.point != Vector3.zero)
            {
                Debug.DrawLine(hit.point, transform.position, Color.red);
                aimRotationTarget.LookAt(hit.point);
            }
            else
            {
                Vector3 dir = transform.forward * 200;
                Debug.DrawLine(dir, transform.position, Color.blue);
                aimRotationTarget.LookAt(dir);
            }
        }
    }

	void OnGUI ()
	{
		GUILayout.BeginVertical ();
		// Release cursor on escape keypress
		if (Input.GetKeyDown (KeyCode.Escape))
			Cursor.lockState = wantedMode = CursorLockMode.None;
			
		switch (Cursor.lockState)
		{
			case CursorLockMode.None:
				GUILayout.Label ("Cursor is normal");
				if (GUILayout.Button ("Lock cursor"))
					wantedMode = CursorLockMode.Locked;
				if (GUILayout.Button ("Confine cursor"))
					wantedMode = CursorLockMode.Confined;
				break;
			case CursorLockMode.Confined:
				GUILayout.Label ("Cursor is confined");
				if (GUILayout.Button ("Lock cursor"))
					wantedMode = CursorLockMode.Locked;
				if (GUILayout.Button ("Release cursor"))
					wantedMode = CursorLockMode.None;
				break;
			case CursorLockMode.Locked:
				GUILayout.Label ("Cursor is locked");
				if (GUILayout.Button ("Unlock cursor"))
					wantedMode = CursorLockMode.None;
				if (GUILayout.Button ("Confine cursor"))
					wantedMode = CursorLockMode.Confined;
				break;
		}

		GUILayout.EndVertical ();

		SetCursorState ();
	}
}
