using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ai.Goap;

public class Player : MonoBehaviour, IStateful {
    public KeyCode upKey = KeyCode.UpArrow;
    public KeyCode downKey = KeyCode.DownArrow;
    public KeyCode leftKey = KeyCode.LeftArrow;
    public KeyCode rightKey = KeyCode.RightArrow;
    public KeyCode shootKey = KeyCode.Mouse0;

    public float speed = 2f;
    public int ammo = 5;

    private Rigidbody2D body;

    void Awake() {
        body = GetComponent<Rigidbody2D>();
    }

    void Update() {
        var velocity = body.velocity;

        if (Input.GetKeyUp(upKey) || Input.GetKeyUp(downKey)) {
            velocity.y = 0;
        }

        if (Input.GetKeyDown(upKey)) {
            velocity.y = speed;
        }

        if (Input.GetKeyDown(downKey)) {
            velocity.y = -speed;
        }

        if (Input.GetKeyUp(leftKey) || Input.GetKeyUp(rightKey)) {
            velocity.x = 0;
        }

        if (Input.GetKeyDown(rightKey)) {
            velocity.x = speed;
        }

        if (Input.GetKeyDown(leftKey)) {
            velocity.x = -speed;
        }



        body.velocity = velocity;
    }

    public State GetState() {
        return null;
    }
}
