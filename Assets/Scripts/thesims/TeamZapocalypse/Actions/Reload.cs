using System.Collections.Generic;
using UnityEngine;
using Ai.Goap;

namespace TeamZapocalypse {
public class Reload : GoapAction {
    private List<IStateful> targets;
    private int reloadedBullets = 3;
    private int maxBullets = 5;

    protected void Awake() {
        AddPrecondition(Item.Ammo.ToString(), CompareType.LessThan, maxBullets);
        AddEffect(Item.Ammo.ToString(), ModificationType.Add, reloadedBullets);
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
        backpack.items[Item.Ammo] += reloadedBullets;

        // Stay at maximum bullets
//        backpack.items[Item.Ammo] = Mathf.Min(backpack.items[Item.Ammo], maxBullets);

        return base.OnDone(agent, context);
    }
}
}