using Ai.Goap;

namespace TeamZapocalypse {
public class Scavenger : Character {
    private readonly WorldGoal worldGoal = new WorldGoal();

    protected override void Awake() {
        base.Awake();
        var goal = new Goal();
        goal["shieldEnergy"] = new Condition(CompareType.MoreThan, 0);
        worldGoal[this] = goal;
    }

    public override WorldGoal CreateGoalState() {
        return worldGoal;
    }
}
}