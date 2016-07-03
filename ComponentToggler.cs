using UnityEngine;
using System.Collections;

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
class ComponentTarget
{
    public MonoBehaviour target;
    public string commentary;
}
