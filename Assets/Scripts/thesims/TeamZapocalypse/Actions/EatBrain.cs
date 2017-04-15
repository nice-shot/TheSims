using UnityEngine;
using System.Collections.Generic;
using Ai.Goap;

namespace TeamZapocalypse {
public class EatBrain : GoapAction {
//    private List<IStateful> targets = new List<IStateful>();

    protected void Awake() {
        AddTargetPrecondition("isSafe", CompareType.Equal, false);
//        AddTargetPrecondition("isAlive", CompareType.Equal, true);
        AddEffect("brains", ModificationType.Add, 1);
        AddEffect("move", ModificationType.Set, true);
//        AddTargetEffect("isAlive", ModificationType.Set, false);
        AddTargetEffect("isSafe", ModificationType.Set, true);
    }

//    protected void Start() {
//
//    }

    public override bool RequiresInRange() {
        return true;
    }

    public override List<IStateful> GetAllTargets(GoapAgent agent) {
        List<IStateful> targets = new List<IStateful>();
        var allCharacters = GetTargets<Character>();
        foreach (Character c in allCharacters) {
            if (c.isAlive) {
                targets.Add(c);
            }
        }
        return targets;
    }

    protected override bool OnDone(GoapAgent agent, WithContext context) {
        var target = (Scavenger)context.target;
        var targetPos = target.transform.position;
        var newZombie = (GameObject)Instantiate(agent.gameObject);
        newZombie.transform.position = targetPos;
        newZombie.transform.parent = agent.gameObject.transform.parent;
        target.isAlive = false;
        var returnVal = base.OnDone(agent, context);
        if (returnVal) {
            Destroy(target.gameObject);
//            Destroy(gameObject);
//            target.gameObject.SetActive(false);
        }
        return returnVal;

    }
}
}

