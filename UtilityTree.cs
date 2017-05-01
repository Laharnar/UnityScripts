using System;
using System.Collections.Generic;

class UtilityNode {
    public Action method;
    List<ArgsFn> scoreFunctions = new List<ArgsFn>();

    public float lastSum = 0;

    public UtilityNode(Action method) {
        this.method = method;
    }

    public UtilityNode(Action method, params ArgsFn[] argsFn) : this(method) {
        scoreFunctions.AddRange(argsFn);
    }

    public UtilityNode(string tag, Action method, params ArgsFn[] argsFn) : this(method) {
        scoreFunctions.AddRange(argsFn);
    }

    public virtual void Score() {
        float sum = 0;
        for (int i = 0; i < scoreFunctions.Count; i++) {
            float s = scoreFunctions[i].Call();
            sum += s;
        }
        lastSum = sum;
    }

    public void Add(Func<float> fn) {
        scoreFunctions.Add(new ArgsFn(fn));
    }
    public void Add(Func<float, float> fn, float fl) {
        scoreFunctions.Add(new ArgsFn(fn, fl));
    }

    public void Add(ArgsFn fn) {
        scoreFunctions.Add(fn);
    }
}

abstract class DecisionNode {
    public List<UtilityNode> choices = new List<UtilityNode>();

    public abstract void Do();

    public DecisionNode AddChoices(params UtilityNode[] choices) {
        this.choices.AddRange(choices);
        return this;
    }
}

class DecisionMaxChoice : DecisionNode {
    public override void Do() {
        int max = -1;
        for (int i = 0; i < choices.Count; i++) {
            choices[i].Score();
            if (max == -1) {
                max = i;
            } else if (choices[i].lastSum > choices[max].lastSum) {
                max = i;
            }
        }

        choices[max].method();
    }

    internal void AddChoice(UtilityNode choice) {
        choices.Add(choice);
    }
}
