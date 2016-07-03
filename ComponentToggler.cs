using UnityEngine;
using System.Collections;

// version 2
public class TransformToggler : MonoBehaviour {

    public ComponentTarget[] targets;
    private int _activePlayer = -1;

    public int ActivePlayer
    {
        get { return _activePlayer; }
        set
        {
            if (targets.Length == 0)
            {
                return;
            }
            _activePlayer = value % targets.Length;
        }
    }

    public void NextTarget()
    {
        if (targets.Length == 0)
	    {
		     return;
	    }

        ActivePlayer++;
        if (ActivePlayer-1 > -1)
	    {
            targets[ActivePlayer-1].target.SetActive(false);
	    }
        targets[ActivePlayer].target.SetActive(true);
    }
}
[System.Serializable]
public class ComponentTarget
{
    public GameObject target;
    public string commentary;
}

// version 1
public abstract class ComponentToggler : MonoBehaviour{

    public ComponentTarget[] targets;
    private int _activePlayer = -1;

    public int ActivePlayer
    {
        get { return _activePlayer; }
        set
        {
            if (targets.Length == 0)
            {
                return;
            }
            _activePlayer = value % targets.Length;
        }
    }

    public void NextTarget()
    {
        ActivePlayer++;
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].target.enabled = false;
        }
        targets[ActivePlayer].target.enabled = true;
    }
}

[System.Serializable]
public class ComponentTarget
{
    public MonoBehaviour target;
    public string commentary;
}
