using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamZapocalypse {
public class ZombieSpawner : MonoBehaviour {
    public GameObject zombiePrefab;
    private int hordeSize = 0;

	// Use this for initialization
	void Start () {
        SummonZombies(1);
	}
	
	// Update is called once per frame
	void Update () {
        int numOfZombies = GameObject.FindGameObjectsWithTag("Zombie").Length;
        if (numOfZombies == 0) {
            hordeSize++;
            SummonZombies(hordeSize);
        }
	}

    void SummonZombies(int numberOfZombies) {
        Transform[] spawnTransforms = GetComponentsInChildren<Transform>();
        for (int i=0; i<numberOfZombies; i++) {
            Transform chosenTransform = spawnTransforms[Random.Range(0, spawnTransforms.Length)];
            GameObject zombie = (GameObject)Instantiate(zombiePrefab);
            var pos = chosenTransform.position;
            // Z position for characters is always -2
            zombie.transform.position = new Vector3(pos.x, pos.y, -2);
        }
    }
}
}