using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ai.Goap;

public class Player : MonoBehaviour, IStateful {
//    public KeyCode upKey = KeyCode.UpArrow;
//    public KeyCode downKey = KeyCode.DownArrow;
//    public KeyCode leftKey = KeyCode.LeftArrow;
//    public KeyCode rightKey = KeyCode.RightArrow;
    public KeyCode shootKey = KeyCode.Mouse0;

    public float speed = 2f;
    public int ammo = 5;

    private Rigidbody2D body;

    void Awake() {
        body = GetComponent<Rigidbody2D>();
    }

    void Update() {
        var move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        body.velocity = move * speed;
    }

//    public OnTouchDown(

    public State GetState() {
        return null;
    }
}
