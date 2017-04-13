using UnityEngine;
using System.Collections.Generic;
using Ai.Goap;

namespace TeamZapocalypse {
public class EatBrain : GoapAction {
    private List<IStateful> targets = new List<IStateful>();

    protected void Awake() {
        AddTargetPrecondition("isSafe", CompareType.Equal, false);
        AddEffect("brains", ModificationType.Add, 1);
    }

    protected void Start() {
        var allCharacters = GetTargets<Character>();
        foreach (Character c in allCharacters) {
            if (c.isAlive) {
                targets.Add(c);
            }
        }
    }

    public override bool RequiresInRange() {
        return true;
    }

    public override List<IStateful> GetAllTargets(GoapAgent agent) {
        return targets;
    }
}
}

