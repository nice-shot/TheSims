using System.Collections.Generic;
using Ai.Goap;

namespace TeamFirewood {
/// <summary>
/// Sacrifices an animal in the shrine
/// </summary>
public class SacrificeAction : GoapAction {
    private const Item resource = Item.Animal;
    public int amountToSacrifice;
    private List<IStateful> targets;

    protected virtual void Awake() {
        AddPrecondition(resource.ToString(), CompareType.MoreThanOrEqual, amountToSacrifice);
        AddEffect("sacrifice", ModificationType.Set, true);
        AddEffect(resource.ToString(), ModificationType.Add, -amountToSacrifice);
    }

    protected void Start() {
        targets = PointOfInterest.GetPointOfInterest(PointOfInterestType.Shrine);
    }

    public override bool RequiresInRange() {
        return true;
    }

    public override List<IStateful> GetAllTargets(GoapAgent agent) {
        return targets;
    }

    protected override bool OnDone(GoapAgent agent, WithContext context) {
        // Done sacrificing
        var backpack = agent.GetComponent<Container>();
        backpack.items[resource] -= amountToSacrifice;
        return base.OnDone(agent, context);
    }
}
}