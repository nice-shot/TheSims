using UnityEngine;
using Ai.Goap;

namespace TeamZapocalypse {
public class Zombie : Character {
    private readonly WorldGoal worldGoal = new WorldGoal();
    public int health = 5;
    public float increaseSpeedAmount = 0.1f;
    public float increaseSpeedSecondsDiff = 3f;

    private float lastSpeedChange;

    protected override void Awake() {
        base.Awake();
        lastSpeedChange = Time.time;
        // Different zombies will be a little slower or fast depending on this:w
        increaseSpeedSecondsDiff = Random.Range(increaseSpeedSecondsDiff - 2, increaseSpeedSecondsDiff + 2);
        isAlive = false;
        var goal = new Goal();
//        goal["brains"] = new Condition(CompareType.MoreThan, 0);
        goal["move"] = new Condition(CompareType.Equal, true);
        worldGoal[this] = goal;
    }

    protected override void Update() {
        if (Time.time - lastSpeedChange > increaseSpeedSecondsDiff) {
            moveSpeed += increaseSpeedAmount;
            lastSpeedChange = Time.time;
        }
        base.Update();
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

