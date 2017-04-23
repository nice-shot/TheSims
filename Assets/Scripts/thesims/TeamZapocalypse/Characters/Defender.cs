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

//        // Move towards the NextAction's target.
//        float step = moveSpeed * Time.deltaTime;
//        var target = nextAction.target as Component;
//        // NOTE: We must cast to Vector2, otherwise we'll compare the Z coordinate
//        //       which does not have to match!
//        var position = target.transform.position;
//        // Keep Z position to not get below background
//        position.z = transform.position.z;
//
//        // TODO: Move by setting the velocity of a rigid body to allow collisions.
//        transform.position = Vector3.MoveTowards(transform.position, position, step);
//
//        if (position.Approximately(transform.position)) {
//            // We are at the target location, we are done.
//            nextAction.isInRange = true;
//            return true;
//        }
//        return false;
    }
}
}

