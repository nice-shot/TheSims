using System.Collections.Generic;
using Ai.Goap;

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
        return targets;
    }

//    protected override bool OnDone(GoapAgent agent, WithContext context) {
//        var backpack = agent.GetComponent<Container>();
//        var shelter = (Shelter)context.target;
//        shelter.AddFuel(backpack.items[resource]);
//        backpack.items[resource] = 0;
//        return base.OnDone(agent, context);
//    }
}
}
