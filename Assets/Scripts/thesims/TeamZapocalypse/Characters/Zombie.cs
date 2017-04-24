using UnityEngine;
using Ai.Goap;

namespace TeamZapocalypse {
public class Zombie : Character {
    private readonly WorldGoal worldGoal = new WorldGoal();
    public int health = 5;

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
        
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("bullet")) {
            Destroy(other.gameObject);
            health--;
            if (health <= 0) {
                Debug.Log("<color=green>Zombie died...</color>");
                Destroy(gameObject);
            }
            gotAbort = true;
        }
    }
}
}

