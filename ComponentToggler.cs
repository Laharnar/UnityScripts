using UnityEngine;
using System.Collections;

public abstract class ComponentToggler {

    MonoBehaviour[] targets;
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
            targets[i].enabled = false;
        }
        targets[ActivePlayer].enabled = true;
    }

    
}
