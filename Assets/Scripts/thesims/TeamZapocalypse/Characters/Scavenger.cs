using UnityEngine;
using Ai.Goap;

namespace TeamZapocalypse {
public class Scavenger : Character {
    private readonly WorldGoal worldGoal = new WorldGoal();

    protected override void Awake() {
        base.Awake();
        isAlive = true;
        var goal = new Goal();
        goal["shieldEnergy"] = new Condition(CompareType.MoreThan, 0);
//        goal["health"] = new Condition(CompareType.MoreThan
//        goal["safety"] = new Condition(CompareType.Equal, true);
        worldGoal[this] = goal;
    }

    public override WorldGoal CreateGoalState() {
        return worldGoal;
    }
}
}