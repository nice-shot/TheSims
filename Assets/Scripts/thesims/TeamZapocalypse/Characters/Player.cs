﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ai.Goap;

public class Player : MonoBehaviour, IStateful {
//    public KeyCode upKey = KeyCode.UpArrow;
//    public KeyCode downKey = KeyCode.DownArrow;
//    public KeyCode leftKey = KeyCode.LeftArrow;
//    public KeyCode rightKey = KeyCode.RightArrow;
    public KeyCode shootKey = KeyCode.Mouse0;
    public GameObject bulletPrefab;

    public float speed = 2f;
    public int maxAmmo = 5;
    public int currentAmmo = 5;
    public float bulletSpeed = 5f;
    public float bulletLifespan = 0.6f;

    private Rigidbody2D body;
    private bool inShelter;

    public State GetState() {
        return null;
    }

    void Awake() {
        body = GetComponent<Rigidbody2D>();
    }

    void Update() {
        var move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        body.velocity = move * speed;

        if (Input.GetKeyDown(shootKey) && CanShoot()) {
            currentAmmo--;
            var bullet = (GameObject)Instantiate(
                bulletPrefab,
                gameObject.transform
            );

            bullet.transform.position = gameObject.transform.position;

            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 relativeVec = mouseWorldPos - gameObject.transform.position;

            bullet.GetComponent<Rigidbody2D>().velocity = relativeVec.normalized * bulletSpeed;
            Destroy(bullet, bulletLifespan);
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.name == "Shelter") {
            inShelter = false;
        }
    }

    void OnTriggerStay2D(Collider2D collider) {
        if (collider.name == "Shelter") {
            inShelter = true;
            currentAmmo = maxAmmo;
        }
    }

    private bool CanShoot() {
        return (!inShelter && currentAmmo > 0);
    }


}
