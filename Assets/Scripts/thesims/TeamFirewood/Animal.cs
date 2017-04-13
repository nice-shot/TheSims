using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamFirewood {
public class Animal : MonoBehaviour {
    [SerializeField] float moveSpeed = 1f;
    	
	// Randomly moves somewhere
	void Update () {
        var pos = gameObject.transform.position;
        pos.x += Random.Range(0f, moveSpeed * Time.deltaTime);
        pos.y += Random.Range(0f, moveSpeed * Time.deltaTime);
        gameObject.transform.position = pos;
	}
}
}