using UnityEngine;
using System.Collections.Generic;
using Infra.Utils;
using Ai.AStar;
using Ai.Goap;

namespace TeamZapocalypse {
/// <summary>
/// A general labourer class.
/// Inherit from this for specific Labourer classes and implement the
/// CreateGoalState() method that will populate the goal for the GOAP planner.
/// </summary>
public abstract class Character : GoapAgent {
    [SerializeField] Container backpack;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] TextBubble textBubble;

    public bool isAlive;
    private readonly State state = new State();

    protected override void Awake() {
        base.Awake();

        foreach (var item in EnumUtils.EnumValues<Item>()) {
            if (item == Item.None)
                continue;
            state[item.ToString()] = new StateValue(backpack.items[item]);
        }
//        state["hasTool"] = new StateValue(backpack.tool != null);
    }

    protected void Start() {
        if (backpack == null) {
            backpack = gameObject.GetComponent<Container>();
        }
    }

    public override State GetState() {
        foreach (var item in EnumUtils.EnumValues<Item>()) {
            if (item == Item.None)
                continue;
            state[item.ToString()].value = backpack.items[item];
        }
//        state["hasTool"].value = backpack.tool != null;
        state["x"] = new StateValue(transform.position.x);
        state["y"] = new StateValue(transform.position.y);

        return state;
    }

    public override void PlanFailed(WorldGoal failedGoal) {
        // If this happens for too long, there is probably a bug in the actions,
        // goals or world setup.
        // TODO: Make sure the world state has changed before running the same
        //       goal again.
        // TODO: Support multiple goals and select the next one.
        textBubble.SetText("...");
    }

    public override void PlanFound(WorldGoal goal, Queue<ITransition> actions) {
        // Yay we found a plan for our goal!
        Debug.Log("<color=green>Plan found</color> " + GoapAgent.PrettyPrint(actions));
    }

    public override void AboutToDoAction(GoapAction.WithContext action) {
        textBubble.SetText(action.actionData.name);
    }

    public override void ActionsFinished() {
        // Everything is done, we completed our actions for this gool. Hooray!
        Debug.Log("<color=blue>Actions completed</color>");
        textBubble.SetText("Job's Done!");
    }

    public override void PlanAborted(GoapAction.WithContext aborter) {
        // An action bailed out of the plan. State has been reset to plan again.
        // Take note of what happened and make sure if you run the same goal
        // again that it can succeed.
        Debug.Log("<color=red>Plan Aborted</color> " + aborter);
        textBubble.SetText("Hmp!");
    }

    public override bool MoveAgent(GoapAction.WithContext nextAction) {
        // Move towards the NextAction's target.
        float step = moveSpeed * Time.deltaTime;
        var target = nextAction.target as Component;
        // NOTE: We must cast to Vector2, otherwise we'll compare the Z coordinate
        //       which does not have to match!
        var position = (Vector2)target.transform.position;
        // TODO: Move by setting the velocity of a rigid body to allow collisions.
        transform.position = Vector2.MoveTowards(transform.position, position, step);

        if (position.Approximately(transform.position)) {
            // We are at the target location, we are done.
            nextAction.isInRange = true;
            return true;
        }
        return false;
    }
}
}