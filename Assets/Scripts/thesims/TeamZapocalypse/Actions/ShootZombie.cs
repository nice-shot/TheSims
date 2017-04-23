using System.Collections.Generic;
using UnityEngine;
using Ai.Goap;

namespace TeamZapocalypse {
public class ShootZombie : GoapAction {
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;
    public float bulletLifespan = 0.6f;

    private List<IStateful> targets;

    protected void Awake() {
        AddPrecondition(Item.Ammo.ToString(), CompareType.MoreThan, 0);
        AddEffect("killCount", ModificationType.Add, 1);
    }

    protected void Start() {
        targets = GetTargets<Zombie>();
    }

    public override bool RequiresInRange() {
        return true;
    }

    public override List<IStateful> GetAllTargets(GoapAgent agent) {
        return targets;
    }

    protected override bool OnDone(GoapAgent agent, WithContext context) {
        var backpack = agent.GetComponent<Container>();
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            gameObject.transform
        );

        bullet.transform.position = gameObject.transform.position;
        var target = context.target as Component;
        Vector2 relativeVec = target.transform.position - agent.transform.position;

        bullet.GetComponent<Rigidbody2D>().velocity = relativeVec.normalized * bulletSpeed;
        Destroy(bullet, bulletLifespan);

        backpack.items[Item.Ammo]--;

        return base.OnDone(agent, context);
    }
}
}

