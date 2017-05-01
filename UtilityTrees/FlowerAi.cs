using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*example for ai*/
public class FlowerAi : MonoBehaviour {

    /// <summary>
    /// When player is in range, create gas.
    /// Gas creates units that will stay inside the gas.
    /// Gas can dissapear when player is far enough
    /// </summary>

    public Transform targetPlayer;

    public Spawner bugs = new Spawner();
    Timer gasTimer = new Timer();

    List<DecisionMaxChoice> ai = new List<DecisionMaxChoice>();

    float timeSinceEntrance;

    // Use this for initialization
    void Awake() {

        ai.Add((DecisionMaxChoice) new DecisionMaxChoice().AddChoices(
            new UtilityNode("poisonArea", CreateGas,
                new ArgsFn(InRange, 25f)),
            new UtilityNode("becomeNeutral", StopPoison,
                new ArgsFn(FlatScore, 0f))// this valule is directly 20-range when to stop
            )
        );
        ai.Add((DecisionMaxChoice) new DecisionMaxChoice().AddChoices(
            new UtilityNode("callBugs", CreateUnits,
                new ArgsFn(InRange, 30f), // will start spawning in range of 14+5-unit count
                new ArgsFn(UnitCount)),
            new UtilityNode("becomeNeutral", DoNothing,
                new ArgsFn(FlatScore, 0f))// this valule is directly 20-range when to stop
            )
        );
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, 25);
        Gizmos.DrawWireSphere(transform.position, 30);
        

    }

    // Update is called once per frame
    void Update() {
        foreach (var item in ai) {
            item.Do();
        }
    }

    void StopPoison() { }

    void DoNothing() {
        timeSinceEntrance = 0;
    }

    void CreateGas() {
        // TODO: play particle system
        if (gasTimer.IsTime()) {
            Debug.Log("making gas");
            ApplyHalucinations();
            gasTimer.Next(1);
        }
    }

    void CreateUnits() {
        Debug.Log("making units "+ bugs.instances.Count);
        bugs.Spawn();
    }
    
    float FlatScore(float score) {
        return score;
    }

    float UnitCount() {
        // Note: don't use it multiple times, because of timeSinceEntrance
        float perUnitValue = -5;// closer player gets, more gets spawned, up to 4
        int baseValue = 5;
        float result = baseValue + bugs.instances.Count * perUnitValue+ timeSinceEntrance;
        timeSinceEntrance += Time.deltaTime;
        return result;
    }

    // more than range dist -> out of range, more than 100 when out of range -> stop showing gas 
    float InRange(float range) {
        float baseValue = range;
        return baseValue - Vector3.Distance(transform.position, targetPlayer.position);
    }

    void ApplyHalucinations() {
        // apply debuff on player, reduce movement speed for a few seconds.
        targetPlayer.GetComponent<Unit>().statusEffects.Mix("Slow", 0.9f, 4);
    }
}

public class Timer {
    float t;

    public bool IsTime() {
        return t < Time.time;
    }

    public void Next(float rate) {
        t = Time.time + rate;
    }

    public void NextAt(float time) {
        t = time;
    }
}


[System.Serializable]
public class Spawner {
    public Transform pref;
    internal List<Transform> instances = new List<Transform>();

    Timer timer = new Timer();

    float rate = 2f;
    System.Action spawn;

    public Spawner() {
        spawn = DefaultSpawnFn;
    }

    public Spawner(float rate) {
        this.rate = rate;
        spawn = DefaultSpawnFn;
    }

    public Spawner(float rate, System.Action spawnFn) {
        this.rate = rate;
        spawn = spawnFn;
    }

    void DefaultSpawnFn() {
        instances.Add(GameObject.Instantiate(pref));
    }

    public void Spawn() {
        if (timer.IsTime()) {
            if (spawn == null)
                spawn = DefaultSpawnFn;
            spawn();
            timer.Next(rate);
        }
    }
}
