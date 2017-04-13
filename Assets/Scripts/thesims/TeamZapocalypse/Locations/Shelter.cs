using UnityEngine;
using Ai.Goap;

namespace TeamZapocalypse {
public class Shelter : MonoBehaviour, IStateful {

    public float shieldEnergy = 60f;
    public GameObject shield;

    private const float energyDeplitionRate = 0.2f;

    protected void Awake() {
        CheckShield();
    }

    protected void Update() {
        shieldEnergy = Mathf.Max(0, shieldEnergy - Time.deltaTime * energyDeplitionRate);
        CheckShield();
    }

    private void CheckShield() {
        if (shieldEnergy > 10f) {
            shield.SetActive(true);
        } else {
            shield.SetActive(false);
        }
    }

    public State GetState() {
        // TODO: Add limited resources
        return null;
    }
}
}