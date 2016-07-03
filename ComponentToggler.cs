using UnityEngine;
using System.Collections;

// version 3, automaticaly maps in queue by -1, and allows custom maps
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
        else
        {
            targets[targets.Length - 1].target.SetActive(false);
        }
        targets[ActivePlayer].target.SetActive(true);
    }

    void Start()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].nextTarget == -1)
	        {
                targets[i].nextTarget = (i + 1) % targets.Length;
            }
            else
            {
                targets[i].nextTarget = targets[i].nextTarget % (targets.Length);
            }
        }

        NextTarget();
    }

}

[System.Serializable]
public class ComponentTarget
{
    public GameObject target;
    public string commentary;
    public int nextTarget = -1; // -1 means next target in queue
}


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
