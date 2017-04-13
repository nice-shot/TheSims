using System.Collections.Generic;
using Ai.Goap;

namespace TeamZapocalypse {
public class DropOffFuel : GoapAction {
    private Item resource = Item.Fuel;
    private int energyFromFuel = 10;
    private List<IStateful> targets;

    protected void Awake() {
        AddPrecondition(resource.ToString(), CompareType.MoreThanOrEqual, 1);
        AddEffect(resource.ToString(), ModificationType.Set, 0);
        AddEffect("shieldEnergy", ModificationType.Add, energyFromFuel);
        //        AddTargetPrecondition(resource.ToString(), CompareType.MoreThanOrEqual, amountToTake);
        //        AddTargetEffect(resource.ToString(), ModificationType.Add, -amountToTake);
    }

    protected void Start() {
        targets = GetTargets<Shelter>();
    }

    public override bool RequiresInRange() {
        return true;
    }

    public override List<IStateful> GetAllTargets(GoapAgent agent) {
        return targets;
    }

    protected override bool OnDone(GoapAgent agent, WithContext context) {
        var backpack = agent.GetComponent<Container>();
        var shelter = (Shelter)context.target;
        shelter.AddFuel(backpack.items[resource]);
        backpack.items[resource] = 0;
        return base.OnDone(agent, context);
    }
}
}

