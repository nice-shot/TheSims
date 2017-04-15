using System.Collections.Generic;
using Ai.Goap;
using UnityEngine;

namespace TeamZapocalypse {
public class WanderAround : GoapAction {
    private List<IStateful> targets;

    protected void Awake() {
        AddEffect("move", ModificationType.Set, true);
    }

    protected void Start() {
        targets = GetTargets<WanderArea>();
    }

    public override bool RequiresInRange() {
        return true;
    }

    public override List<IStateful> GetAllTargets(GoapAgent agent) {
        int selected = Random.Range(0, targets.Count);
        return new List<IStateful> { targets[selected] };
    }
}
}
