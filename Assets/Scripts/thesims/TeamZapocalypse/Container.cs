using UnityEngine;
using System.Collections.Generic;
using Infra.Utils;

namespace TeamZapocalypse {
public enum Item {
    None,
    Fuel,
    Ammo
}

/// <summary>
/// Backpack that holds a tool and resources.
/// </summary>
public class Container : MonoBehaviour {
//    public GameObject tool;
    // TODO: Add each tool as a specific item. Have the blacksmith craft each
    //       tool separately. One goal per tool.
    // TODO: Allow changing the priorities of the blacksmith's goals.
    public Dictionary<Item, int> items = new Dictionary<Item, int> {
        {Item.Fuel, 0},
        {Item.Ammo, 0}
    };

    [SerializeField]
    protected int fuel;
    [SerializeField]
    protected int ammo;

    protected void Awake() {
        // Make sure all new items are defined in the container.
        foreach (var item in EnumUtils.EnumValues<Item>()) {
            if (item == Item.None) continue;
            items[item] = 0;
        }
        items[Item.Fuel] = fuel;
        items[Item.Ammo] = ammo;
    }

    #if DEBUG_CONTAINER
    protected void Update() {
        fuel = items[Item.Fuel];
        ammo = items[Item.Ammo];
    }
    #endif
}
}
