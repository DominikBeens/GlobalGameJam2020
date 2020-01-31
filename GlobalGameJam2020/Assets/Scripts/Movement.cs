﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxMovementSpeed;
    [SerializeField] private Vector2 playerBoundaries;
    public float movementTimer;
    private Rigidbody myRB;

    void Awake() {
        myRB = GetComponent<Rigidbody>();
    }

    void Update() {

        if (movementTimer < maxMovementSpeed && MovementUpdate() == false) {
            movementTimer += Time.deltaTime * 2;
        } else {
            MovementUpdate();
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -playerBoundaries.x, playerBoundaries.x), Mathf.Clamp(transform.position.y, -playerBoundaries.y, playerBoundaries.y), 0);

        if (transform.position.x == -playerBoundaries.x || transform.position.x == playerBoundaries.x) {
            myRB.velocity = new Vector3(0, myRB.velocity.y, myRB.velocity.z);
        }
        if (transform.position.y == -playerBoundaries.y || transform.position.y == playerBoundaries.y) {
            myRB.velocity = new Vector3(myRB.velocity.x,0, myRB.velocity.z);
        }
    }

    bool hasMoved;
    public bool MovementUpdate() {
        hasMoved = false;

        if (Input.GetAxis("Horizontal") > 0.1f) {
            if (movementTimer > 0) {
                myRB.AddForce(Vector3.right * movementSpeed);
                movementTimer -= Time.deltaTime;
            }
            hasMoved = true;
        }

        if (Input.GetAxis("Horizontal") < -0.1f) {
            if (movementTimer > 0) {
                myRB.AddForce(Vector3.left * movementSpeed);
                movementTimer -= Time.deltaTime;
            }
            hasMoved = true;
        }

        if (Input.GetAxis("Vertical") > 0.1f) {
            if (movementTimer > 0) {
                myRB.AddForce(Vector3.up * movementSpeed);
                movementTimer -= Time.deltaTime;
            }
            hasMoved = true;
        }

        if (Input.GetAxis("Vertical") < -0.1f) {
            if (movementTimer > 0) {
                myRB.AddForce(Vector3.down * movementSpeed);
                movementTimer -= Time.deltaTime;
            }
            hasMoved = true;
        }

        return hasMoved;
    }
}