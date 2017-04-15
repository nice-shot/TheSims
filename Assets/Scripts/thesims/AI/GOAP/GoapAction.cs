using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using Infra;
using Infra.Collections;
using Ai.AStar;

namespace Ai.Goap {
public abstract class GoapAction : MonoBehaviour {
    /// <summary>
    /// The cost of performing the action.
    /// </summary>
    [Tooltip("The cost of performing the action")]
    public float cost = 1f;
    public float workDuration;

    private Goal preconditions = new Goal();
    private Goal targetPreconditions = new Goal();
    private readonly Effects effects = new Effects();
    private readonly Effects targetEffects = new Effects();

    /// <summary>
    /// Default implementation returns the agent as the target.
    /// </summary>
    public virtual List<IStateful> GetAllTargets(GoapAgent agent) {
        var targets = new List<IStateful>();
        targets.Add(agent);
        return targets;
    }

    /// <summary>
    /// Returns false if the action can not be performed.
    /// Override this to implement the affect of completing the action.
    /// </summary>
    protected virtual bool OnDone(GoapAgent agent, WithContext context) {
        context.isDone = true;
        DebugUtils.Log(GetType().Name + " DONE!");
        return true;
    }

    public virtual bool CanDoNow(GoapAgent agent, IStateful target) {
        // We don't know how. We just know it works...
        if (target.ToString() == "null" || target == null) {
            return false;
        }
        var worldState = WorldState.Borrow();
        worldState[agent] = agent.GetState();
        worldState[target] = target.GetState();
        // Since the action is a part of the plan, its preconditions on the agent
        // should apply.
        var doesApply = worldState.IsGoalState(GetIndependentPreconditions(agent));
        // NOTE: If the agent's state is volatile and can change outside of the
        // agent's plan, comment out this assertion.
        DebugUtils.AssertWarning(
            doesApply,
            "WARNING: Possible bug in definition of " + name
            + ". The agent " + agent.name + " planned to do it but now its state does not allow it");
        // Need to check that the target's state still apply to the preconditions
        // defined by the action.
        doesApply = doesApply && worldState.IsGoalState(GetDependentPreconditions(agent, target));
        worldState.ReturnSelf();
        return doesApply;
    }

    /// <summary>
    /// Does this action need to be within range of a target game object?
    /// If not then the moveTo state will not need to run for this action.
    /// </summary>
    public abstract bool RequiresInRange();

    public void AddPrecondition(string key, CompareType comparison, object value) {
        preconditions[key] = new Condition(comparison, value);
    }

    public void AddTargetPrecondition(string key, CompareType comparison, object value) {
        targetPreconditions[key] = new Condition(comparison, value);
    }

    public void AddEffect(string key, ModificationType modifier, object value) {
        effects[key] = new Effect(modifier, value);
    }

    public void AddTargetEffect(string key, ModificationType modifier, object value) {
        targetEffects[key] = new Effect(modifier, value);
    }

    public static List<IStateful> GetTargets<T>() where T : Component, IStateful {
        var candidates = FindObjectsOfType<T>();
        var list = new List<IStateful>(candidates.Length);
        foreach (var item in candidates) {
            list.Add(item);
        }
        return list;
    }

    /// <summary>
    /// Returns a WorldGoal that contains all the preconditions that the agent
    /// must satisfy.
    /// </summary>
    public virtual WorldGoal GetIndependentPreconditions(IStateful agent) {
        var worldPreconditions = WorldGoal.Borrow();
        if (preconditions.Count > 0) {
            worldPreconditions[agent] = preconditions;
        }
        return worldPreconditions;
    }

    /// <summary>
    /// Returns a WorldGoal that contains all the preconditions that the target
    /// of the action must satisfy.
    /// </summary>
    public virtual WorldGoal GetDependentPreconditions(IStateful agent, IStateful target) {
        var worldPreconditions = WorldGoal.Borrow();
        if (targetPreconditions.Count > 0) {
            worldPreconditions[target] = targetPreconditions;
        }
        return worldPreconditions;
    }


    /// <summary>
    /// Returns the effects that should be applied to the agent and the target
    /// of the action.
    /// </summary>
    public virtual WorldEffects GetDependentEffects(IStateful agent, IStateful target) {
        var worldEffects = WorldEffects.Borrow();
        if (effects.Count > 0) {
            worldEffects[agent] = effects;
        }
        if (targetEffects.Count > 0) {
            worldEffects[target] = targetEffects;
        }
        return worldEffects;
    }

    public override string ToString() {
        return name;
    }

    [Serializable]
    public class WithContext : ITransition {
        private static ObjectPool<WithContext> pool = new ObjectPool<WithContext>(175, 25);
        private static int lastPoolSize = 175;

        public GoapAction actionData;
        [Tooltip("The agent performing the action")]
        public GoapAgent agent;
        /// <summary>
        /// The target the action acts upon. Optional.
        /// </summary>
        [Tooltip("The target the action acts upon. Optional")]
        public IStateful target;

        private float startTime = 0;

        /// <summary>
        /// Are we in range of the target?
        /// The MoveTo state will set this and it gets reset each time this action is performed.
        /// </summary>
        public bool isInRange;

        public bool isDone;

        public static void ReportLeaks() {
            var poolSize = pool.Count;
            if (poolSize > lastPoolSize) {
                DebugUtils.LogError("WithContext pool size: " + poolSize);
                lastPoolSize = poolSize;
            }
        }

        public static WithContext Borrow(GoapAction action, GoapAgent agent, IStateful target) {
            var actionWithContext = pool.Borrow();
            actionWithContext.actionData = action;
            actionWithContext.agent = agent;
            actionWithContext.target = target;
            actionWithContext.startTime = 0;
            actionWithContext.isInRange = false;
            actionWithContext.isDone = false;
            return actionWithContext;
        }

        public void ReturnSelf() {
            pool.Return(this);
        }

        public ITransition Clone() {
            return Borrow(actionData, agent, target);
        }

        /// <summary>
        /// Apply the transition to given state in order to calculate the next state.
        /// </summary>
        /// <param name="state">The state to transition from.</param>
        /// <param name="inPlace">Set to true if it's ok to modify the given state.</param>
        public IState ApplyToState(IState state, bool inPlace = false) {
            // Copy state.
            var worldState = state as WorldState;
            if (worldState == null) {
                return ApplyInReverse(state as RegressiveSearchWorldGoal, inPlace);
            }
            if (!inPlace) {
                var prevWorldState = worldState;
                worldState = WorldState.Borrow();
                foreach (var kvp in prevWorldState) {
                    worldState[kvp.Key] = kvp.Value;
                }
            }
            // Apply change.
            var effects = actionData.GetDependentEffects(agent, target);
            foreach (var change in effects) {
                // Add any new stateful objects that are affected by this transition
                // to the new state.
                if (!worldState.ContainsKey(change.Key)) {
                    worldState[change.Key] = change.Key.GetState();
                }
                worldState[change.Key] = worldState[change.Key].ApplyEffects(change.Value);
            }
            effects.ReturnSelf();
            // Apply movement.
            if (actionData.RequiresInRange()) {
                var agentState = worldState[agent];
                var obj = target as Component;
                var x = StateValue.NormalizeValue(obj.transform.position.x);
                var y = StateValue.NormalizeValue(obj.transform.position.y);
                agentState["x"] = new StateValue(x);
                agentState["y"] = new StateValue(y);
            }
            return worldState;
        }

        private RegressiveSearchWorldGoal ApplyInReverse(RegressiveSearchWorldGoal goal, bool inPlace = false) {
            var effects = actionData.GetDependentEffects(agent, target);
            goal = goal.RegressWorldGoal(effects, inPlace);
            effects.ReturnSelf();
            goal.AddPreconditions(actionData.GetIndependentPreconditions(agent));
            goal.AddPreconditions(actionData.GetDependentPreconditions(agent, target));
            // Apply movement.
            if (actionData.RequiresInRange()) {
                // Don't add the position as a goal because we don't have a
                // just MoveAction to satisfy this goal. Instead, we calculate
                // this cost as part of the heuristic.
                var obj = target as Component;
                goal.agentGoalPosition = obj.transform.position;
            }
            return goal;
        }

        /// <summary>
        /// Calculates the cost of the transition from the given state.
        /// </summary>
        public float CalculateCost(IState fromState) {
            var worldState = fromState as WorldState;
            if (worldState == null) {
                // The action is used in regressive search, so the state is
                // actually a goal.
                return RegressiveSearchCost(fromState as RegressiveSearchWorldGoal);
            }
            // Calculate travel cost.
            var travelCost = 0f;
            if (actionData.RequiresInRange()) {
                var agentState = worldState[agent];
                var currentPosition = new Vector2((int)agentState["x"].value, (int)agentState["y"].value);
                var obj = target as Component;
                var travelVector = (Vector2)obj.transform.position - currentPosition;
                travelCost = travelVector.magnitude;
                //DebugUtils.Log(travelCost + " to " + obj.name);
            }
            return actionData.cost + travelCost;
        }

        private float RegressiveSearchCost(RegressiveSearchWorldGoal goal) {
            DebugUtils.Assert(goal != null, "CalculateCost used with something other than WorldState or RegressiveSearchWorldGoal");
            // Calculate travel cost.
            var travelCost = 0f;
            if (actionData.RequiresInRange()) {
                if (goal.agentGoalPosition != null) {
                    var goalPosition = (Vector2)goal.agentGoalPosition;
                    var obj = target as Component;
                    var travelVector = (Vector2)obj.transform.position - goalPosition;
                    travelCost = travelVector.magnitude;
                    //DebugUtils.Log(travelCost + " to " + obj.name);
                }
            }
            return actionData.cost + travelCost;
        }

        /// <summary>
        /// Run the action.
        /// Returns True if the action performed successfully or false if
        /// something happened and it can no longer perform. In this case the
        /// action queue should clear out and the goal cannot be reached.
        /// </summary>
        public bool Perform(GoapAgent agent) {
            if (Mathf.Approximately(startTime, 0f)) {
                if (!actionData.CanDoNow(agent, target)) {
                    return false;
                }
                startTime = Time.time;
            }

            if (Time.time - startTime > actionData.workDuration) {
                if (!actionData.CanDoNow(agent, target)) {
                    return false;
                }
                DebugUtils.Log(GetType().Name + " Time is up - am I done?");
                return actionData.OnDone(agent, this);
            }
            return true;
        }

        public override string ToString() {
            var s = new StringBuilder();
            return s.Append(actionData.name).Append('@').Append((target as Component).name).ToString();
        }
    }
}
}
