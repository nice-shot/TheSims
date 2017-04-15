using Ai.Goap;

namespace TeamZapocalypse {
public class Zombie : Character {
    private readonly WorldGoal worldGoal = new WorldGoal();

    protected override void Awake() {
        base.Awake();
        isAlive = false;
        var goal = new Goal();
//        goal["brains"] = new Condition(CompareType.MoreThan, 0);
        goal["move"] = new Condition(CompareType.Equal, true);
        worldGoal[this] = goal;
    }

    public override WorldGoal CreateGoalState() {
        return worldGoal;
    }

//    public WorldGoal CreateAlternativeGoal() {
//        var goal = new Goal();
//        goal["wander"] = new Condition(CompareType.Equal, true);
//        worldGoal[this] = goal;
//        return worldGoal;
//    }
//
//    public override void PlanFailed(WorldGoal failedGoal) {
//        CreateAlternativeGoal();
//
//        base.PlanFailed(failedGoal);
//    }
//
//    public override void ActionsFinished() {
//        var goal = new Goal();
//        goal["brains"] = new Condition(CompareType.MoreThan, 0);
//        worldGoal[this] = goal;
//    }

}
}

