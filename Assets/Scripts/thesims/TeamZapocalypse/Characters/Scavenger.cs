﻿using UnityEngine;
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

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.name == "Shelter") {
            Debug.Log("You are not safe!");
            SetIsSafe(false);
        }
    }

    void OnTriggerStay2D(Collider2D collider) {
        if (collider.name == "Shelter") {
            var shelter = collider.gameObject.GetComponent<Shelter>();
            if (shelter.shield.activeInHierarchy) {
                Debug.Log("You are safe!");
                SetIsSafe(true);
            }

        }
    }
}
}