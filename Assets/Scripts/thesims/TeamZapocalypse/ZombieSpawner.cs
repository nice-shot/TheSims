using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamZapocalypse {
public class ZombieSpawner : MonoBehaviour {
    // Actually not a prefab - need an inactive zombie from the game scene
    public GameObject zombiePrefab;
    public int zombieIncreaseSize = 2;
    private int hordeSize = 0;

	// Use this for initialization
	void Start () {
        SummonZombies(1);
	}
	
	// Update is called once per frame
	void Update () {
        int numOfZombies = GameObject.FindGameObjectsWithTag("Zombie").Length;
        if (numOfZombies == 0) {
            hordeSize += zombieIncreaseSize;
            SummonZombies(hordeSize);
        }
	}

    void SummonZombies(int numberOfZombies) {
        Transform[] spawnTransforms = GetComponentsInChildren<Transform>();
        for (int i=0; i<numberOfZombies; i++) {
            // The first transform position will be the script which is (0,0,0)
            Transform chosenTransform = spawnTransforms[Random.Range(1, spawnTransforms.Length)];
            GameObject zombie = (GameObject)Instantiate(zombiePrefab);
            zombie.SetActive(true);
            var pos = chosenTransform.position;
            // Z position for characters is always -2
            zombie.transform.position = new Vector3(pos.x, pos.y, -2);
        }
    }
}
}