using UnityEngine;
using System.Collections.Generic;
using Ai.Goap;

namespace TeamFirewood {
public enum PointOfInterestType {
    None,
    ChoppingBlock,
    Forge,
    IronRock,
    SupplyPile,
    Tree,
    Branches,
	Shrine,
    Animal
}

public class PointOfInterest : MonoBehaviour, IStateful {
    public PointOfInterestType type;

    /// <summary>
    /// Default implementation returns no state.
    /// </summary>
    public virtual State GetState() {
        return null;
    }

    /// <summary>
    /// Returns all the points of interest of a given type.
    /// This is expensive so better cache the result on Start.
    /// </summary>
    public static List<IStateful> GetPointOfInterest(PointOfInterestType type) {
        var targets = GoapAction.GetTargets<PointOfInterest>();
        for (int i = targets.Count - 1; i >= 0; i--) {
            var point = targets[i] as PointOfInterest;
            if (point.type != type) {
                targets.RemoveAt(i);
            }
        }
        return targets;
    }
}
}