using Ai.Goap;

namespace TeamZapocalypse {
public class Zombie : Character {
    private readonly WorldGoal worldGoal = new WorldGoal();

    protected override void Awake() {
        base.Awake();
        isAlive = false;
        var goal = new Goal();
        goal["brains"] = new Condition(CompareType.MoreThan, 0);
        worldGoal[this] = goal;
    }

    public override WorldGoal CreateGoalState() {
        return worldGoal;
    }

}
}

