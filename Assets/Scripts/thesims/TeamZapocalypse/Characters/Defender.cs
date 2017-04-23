using UnityEngine;
using Ai.Goap;

namespace TeamZapocalypse {
public class Defender : Character {
    private readonly WorldGoal worldGoal = new WorldGoal();

    protected override void Awake() {
        base.Awake();
        isAlive = true;
        var goal = new Goal();
        goal["killCount"] = new Condition(CompareType.MoreThan, 0);
        worldGoal[this] = goal;
    }

    public override WorldGoal CreateGoalState() {
        return worldGoal;
    }

    public override bool MoveAgent(GoapAction.WithContext nextAction) {
        if (nextAction.actionData.name == "ShootZombie") {
            var target = nextAction.target as Component;

            Vector3 distance = target.transform.position - transform.position;


            if (distance.sqrMagnitude < 4) {
                nextAction.isInRange = true;
                return true;
            }
        }

        return base.MoveAgent(nextAction);
    }
}
}

