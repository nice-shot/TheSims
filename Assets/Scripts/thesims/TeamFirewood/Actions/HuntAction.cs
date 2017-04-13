using System.Collections.Generic;
using Ai.Goap;

namespace TeamFirewood {
/// <summary>
/// Picks up an animal
/// </summary>
public class HuntAction : GoapAction {
    private const Item resource = Item.Animal;
    public int amountToHunt;
    private List<IStateful> targets;

    protected virtual void Awake() {
        AddEffect(resource.ToString(), ModificationType.Add, amountToHunt);
    }

    protected void Start() {
        targets = PointOfInterest.GetPointOfInterest(PointOfInterestType.Animal);
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
        backpack.items[resource] += amountToHunt;
        return base.OnDone(agent, context);
    }
}
}