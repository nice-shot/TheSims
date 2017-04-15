using UnityEngine;
using System.Collections.Generic;
using Ai.Goap;
using UnityEngine.SceneManagement;

namespace TeamZapocalypse {
public class EatBrain : GoapAction {
//    private List<IStateful> targets = new List<IStateful>();

    protected void Awake() {
        AddTargetPrecondition("isSafe", CompareType.Equal, false);
//        AddTargetPrecondition("isAlive", CompareType.Equal, true);
        AddEffect("brains", ModificationType.Add, 1);
        AddEffect("move", ModificationType.Set, true);
    }

//    protected void Start() {
//
//    }

    public override bool RequiresInRange() {
        return true;
    }

    public override List<IStateful> GetAllTargets(GoapAgent agent) {
        List<IStateful> targets = new List<IStateful>();
        var allCharacters = GetTargets<Character>();
        foreach (Character c in allCharacters) {
            if (c.isAlive) {
                targets.Add(c);
            }
        }
        targets.Add(GetTargets<Player>()[0]);
        return targets;
    }

    protected override bool OnDone(GoapAgent agent, WithContext context) {
        var target = (MonoBehaviour)context.target;
        var targetPos = target.transform.position;
        if (target.gameObject.name == "Player") {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        } else {
            var newZombie = (GameObject)Instantiate(agent.gameObject);
            newZombie.transform.position = targetPos;
            newZombie.transform.parent = agent.gameObject.transform.parent;
            var returnVal = base.OnDone(agent, context);
            if (returnVal) {
                Destroy(target.gameObject);
            }
            return returnVal;
        }
        return base.OnDone(agent, context);
    }
}
}

