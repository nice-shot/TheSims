using UnityEngine;
using System.Collections.Generic;
using System.Text;
using Ai.AStar;
using Ai.Fsm;

namespace Ai.Goap {
/// <summary>
/// An abstract GOAP agent that can figure out plans based on a goal and a set
/// of actions.
/// </summary>
public abstract class GoapAgent : MonoBehaviour, IStateful, ISearchContext {
// NOTE: DEBUG_PLAN is defined at the Edit > Project Settings > Player > Other Settings.
#if DEBUG_PLAN
    [Tooltip("Used for debugging")]
    [SerializeField] List<string> currentPlan;
#endif

    [Tooltip("List of possible actions the agent can perform")]
    public List<GoapAction> availableActions;

    [SerializeField] bool regressiveSearch;

    private FSM stateMachine = new FSM();
    private Queue<ITransition> currentActions = new Queue<ITransition>();

    protected virtual void Awake() {
        stateMachine.PushState(IdleState);
    }

    protected virtual void Update() {
        stateMachine.Update();
    }

    /// <summary>
    /// Returns the current state of the object for planning purposes.
    /// </summary>
    public abstract State GetState();

    /// <summary>
    /// Return the current goal. The planner will search for a plan that fulfill
    /// it.
    /// </summary>
    public abstract WorldGoal CreateGoalState();

    /// <summary>
    /// No sequence of actions could be found for the supplied goal.
    /// Can be used to select another goal for next time.
    /// </summary>
    public abstract void PlanFailed(WorldGoal failedGoal);

    /// <summary>
    /// A plan was found for the given goal.
    /// The plan is a queue of actions that should be performed in order to
    /// fulfill the goal.
    /// </summary>
    public abstract void PlanFound(WorldGoal goal, Queue<ITransition> actions);

    /// <summary>
    /// About to do the next action.
    /// </summary>
    public abstract void AboutToDoAction(GoapAction.WithContext action);

    /// <summary>
    /// All actions are complete or no valid actions left to be performed.
    /// </summary>
    public abstract void ActionsFinished();

    /// <summary>
    /// One of the actions caused the plan to abort.
    /// </summary>
    /// <param name="aborter">The action that failed the plan.</param>
    public abstract void PlanAborted(GoapAction.WithContext aborter);

    /// <summary>
    /// Moves the agent towards the target of the action.
    /// </summary>
    /// <returns><c>true</c>, if agent is at the target, <c>false</c> otherwise.</returns>
    public abstract bool MoveAgent(GoapAction.WithContext nextAction);

    private bool HasActionPlan() {
        return currentActions.Count > 0;
    }

#region FSM States
    private void IdleState(FSM fsm) {
        // GOAP planning.
        // Get the goal we want to plan for.
        var goal = CreateGoalState();

        // Plan.
        var plan = regressiveSearch ? GoapRegressiveSearchPlanner.Plan(this, goal) : GoapPlanner.Plan(this, goal);
        if (plan != null) {
            // We have a plan, hooray!
            // Clear old plan.
            while (currentActions.Count > 0) {
                var context = currentActions.Dequeue();
                context.ReturnSelf();
            }
            currentActions = plan;
            PlanFound(goal, plan);

#if DEBUG_PLAN
            currentPlan.Clear();
            foreach (GoapAction.WithContext action in currentActions) {
                currentPlan.Add(action.actionData.name + " " + (action.target as Component).name);
            }
#endif

            // Move to PerformAction state.
            fsm.PopState();
            fsm.PushState(PerformActionState);
        } else {
            // Couldn't get a plan.
            Debug.Log("<color=orange>Failed Plan:</color>" + goal);
            PlanFailed(goal);
            // Move back to IdleAction state.
            fsm.PopState();
            fsm.PushState(IdleState);
        }
    }

    private void MoveToState(FSM fsm) {
        // Move the game object.
        var action = currentActions.Peek() as GoapAction.WithContext;
        if (action.actionData.RequiresInRange() && action.target == null) {
            Debug.Log("<color=red>Fatal error:</color> Action requires a target but has none. Planning failed. You did not assign the target in your Action.checkProceduralPrecondition()");
            // Move.
            fsm.PopState();
            // Perform.
            fsm.PopState();
            fsm.PushState(IdleState);
            return;
        }
            
        if (!action.actionData.CanDoNow(this, action.target)) {
            // Target doesn't match preconditions anymore.
            fsm.PopState();
            fsm.PushState(IdleState);
            PlanAborted(action);
            return;
        }

        // Get the agent to move itself.
        if (MoveAgent(action)) {
            fsm.PopState();
        }
    }

    private void PerformActionState(FSM fsm) {
        // Perform the action.
        if (!HasActionPlan()) {
            // No actions to perform.
            Debug.Log("<color=red>Done actions</color>");
            fsm.PopState();
            fsm.PushState(IdleState);
            ActionsFinished();
            return;
        }

        var action = currentActions.Peek() as GoapAction.WithContext;
        if (action.isDone) {
            // The action is done. Remove it so we can perform the next one.
            var context = currentActions.Dequeue();
            context.ReturnSelf();
#if DEBUG_PLAN
            currentPlan.RemoveAt(0);
#endif
        }

        if (HasActionPlan()) {
            // Perform the next action.
            action = currentActions.Peek() as GoapAction.WithContext;
            AboutToDoAction(action);
            bool inRange = !action.actionData.RequiresInRange() || action.isInRange;

            if (inRange) {
                // We are in range, so perform the action.
                bool success = action.Perform(this);

                if (!success) {
                    // Action failed, we need to plan again.
                    fsm.PopState();
                    fsm.PushState(IdleState);
                    PlanAborted(action);
                }
            } else {
                // TODO !!!!!!!!!!!!!!!! Try to check if the action is still possible !!!!!!!!!!!!!!!!!!
//                if (action.actionData.CanDoNow(this, action.target)) {
//                    // We need to move there first.
//                    // Push moveTo state.
//                    fsm.PushState(MoveToState);
//                } else {
//                    // Target doesn't match preconditions anymore.
//                    fsm.PopState();
//                    fsm.PushState(IdleState);
//                    PlanAborted(action);                    
//                }
                // We need to move there first.
                // Push moveTo state.
                fsm.PushState(MoveToState);
            }
            return;
        }
        // No valid actions left, move to Plan state.
        fsm.PopState();
        fsm.PushState(IdleState);
        ActionsFinished();
    }
#endregion

#region Printing GOAP structures
    public static string PrettyPrint(IEnumerable<ITransition> actions) {
        var s = new StringBuilder();
        foreach (var a in actions) {
            s.Append(a).Append("-> ");
        }
        s.Append("GOAL");
        return s.ToString();
    }

    public static string PrettyPrint(GoapAction.WithContext[] actions) {
        var s = new StringBuilder();
        foreach (var a in actions) {
            s.Append(a).Append(", ");
        }
        return s.ToString();
    }
#endregion
}
}
