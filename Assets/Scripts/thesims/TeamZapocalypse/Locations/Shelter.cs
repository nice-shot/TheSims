using UnityEngine;
using UnityEngine.UI;
using Ai.Goap;

namespace TeamZapocalypse {
public class Shelter : MonoBehaviour, IStateful {

    public float shieldEnergy = 60f;
    public Text shieldEnergyText;
    public GameObject shield;

    private readonly State state = new State(); 
    private const float energyDeplitionRate = 0.4f;
    private const float energyFromFuel = 8;
    private const float maxEnergy = 100;

    protected void Awake() {
        state["shieldEnergy"] = new StateValue(shieldEnergy);
        CheckShield();
    }

    protected void Update() {
        shieldEnergy = Mathf.Max(0, shieldEnergy - Time.deltaTime * energyDeplitionRate);
        CheckShield();
    }

    private void CheckShield() {
        state["shieldEnergy"].value = shieldEnergy;
        if (shieldEnergy > 10f) {
            shield.SetActive(true);
            shieldEnergyText.color = Color.blue;
        } else {
            shield.SetActive(false);
            shieldEnergyText.color = Color.red;
        }
        shieldEnergyText.text = "Shield Energy: " + new string('|', (int)shieldEnergy / 4);
    }

    public void AddFuel(int numOfTanks) {
        shieldEnergy += Mathf.Min(numOfTanks * energyFromFuel, maxEnergy);
        CheckShield();
    }

    public State GetState() {
        // TODO: Add limited resources
        return state;
    }
}
}