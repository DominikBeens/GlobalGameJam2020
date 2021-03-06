﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    public static event Action<float> OnChangeChanged = delegate {};

    [SerializeField] private float movementSpeed;
    [SerializeField] private float startMovementSpeed = 1000;
    [SerializeField] private float maxMovementSpeed;
    public float movementTimer;
    private Rigidbody myRB;
    private Camera main;

    //TestGameObject
    public GameObject test;
    public float testSize;
    public bool canMove;

    public GameObject movementObjectDown;
    public GameObject movementObjectDown1;
    public GameObject movementObjectUp;
    public GameObject movementObjectLeft;
    public GameObject movementObjectRight;
    public float timer;

    void Awake() {
        myRB = GetComponent<Rigidbody>();
        main = Camera.main;
        canMove = true;
    }

    void Start() {
        movementTimer = maxMovementSpeed;
    }

    bool moving = false;
    void Update() {
        if (timer > 0) {
            timer -= Time.deltaTime;
            if (timer < 0) {
                InfiMoveie.InfiniMove = false;
            }
        }
        if (!canMove) { return; }
        moving = MovementUpdate();

        if (moving == true && movementTimer - Time.deltaTime * 2 < 0 && movementTimer > 0) {
            movementTimer -= 1;
        }

        if (movementTimer < maxMovementSpeed && moving == false || movementTimer <= 0) {
            movementTimer += Time.deltaTime * 2;
        }

        Vector3 screenPoint = main.WorldToViewportPoint(transform.position);

        if (screenPoint.x < 0.01f) {
            myRB.velocity = new Vector3(1, myRB.velocity.y, myRB.velocity.z);
        } else if (screenPoint.x > 0.99f) {
            myRB.velocity = new Vector3(-1, myRB.velocity.y, myRB.velocity.z);
        }

        if (screenPoint.y < 0.01f) {
            myRB.velocity = new Vector3(myRB.velocity.x, 1, myRB.velocity.z);
        } else if (screenPoint.y > 0.99f) {
            myRB.velocity = new Vector3(myRB.velocity.x, -1, myRB.velocity.z);
        }

        OnChangeChanged(movementTimer / maxMovementSpeed);
    }

    bool hasMoved;
    bool booster;
    float boosterWaitTime = 1.5f;
    float boosterTimer;
    public bool MovementUpdate() {
        hasMoved = false;

        if (boosterTimer >= 0) {
            boosterTimer -= Time.deltaTime;
        }

        if (movementTimer > 0) {

            if (Input.GetAxis("Horizontal") > 0) {
                myRB.AddForce(Vector3.right * movementSpeed);
                movementObjectRight.SetActive(true);
                if (InfiMoveie.InfiniMove == false) {
                    movementTimer -= Time.deltaTime;
                }
                hasMoved = true;
                if (Input.GetButtonDown("Horizontal") && boosterTimer < 0) {
                    myRB.AddForce(Vector3.right * startMovementSpeed, ForceMode.Impulse);
                    boosterTimer = boosterWaitTime;
                    hasMoved = true;
                }
            }

            if (Input.GetAxis("Horizontal") < -0) {
                myRB.AddForce(Vector3.left * movementSpeed);
                movementObjectLeft.SetActive(true);
                if (InfiMoveie.InfiniMove == false) {
                    movementTimer -= Time.deltaTime;
                }
                hasMoved = true;
                if (Input.GetButtonDown("Horizontal") && boosterTimer < 0) {
                    myRB.AddForce(Vector3.left * startMovementSpeed, ForceMode.Impulse);
                    boosterTimer = boosterWaitTime;
                    hasMoved = true;
                }
            }

            if (Input.GetAxis("Vertical") > 0) {
                myRB.AddForce(Vector3.up * movementSpeed);
                movementObjectUp.SetActive(true);
                if (InfiMoveie.InfiniMove == false) {
                    movementTimer -= Time.deltaTime;
                }
                hasMoved = true;
                if (Input.GetButtonDown("Vertical") && boosterTimer < 0) {
                    myRB.AddForce(Vector3.up * startMovementSpeed, ForceMode.Impulse);
                    boosterTimer = boosterWaitTime;
                    hasMoved = true;
                }
            }

            if (Input.GetAxis("Vertical") < -0) {
                myRB.AddForce(Vector3.down * movementSpeed);
                movementObjectDown.SetActive(true);
                movementObjectDown1.SetActive(true);
                if (InfiMoveie.InfiniMove == false) {
                    movementTimer -= Time.deltaTime;
                }
                hasMoved = true;
                if (Input.GetButtonDown("Vertical") && boosterTimer < 0) {
                    myRB.AddForce(Vector3.down * startMovementSpeed, ForceMode.Impulse);
                    boosterTimer = boosterWaitTime;
                    hasMoved = true;
                }
            }

        }
        return hasMoved;
    }

    public void SetVelocity(Vector3 direction) {
        myRB.velocity = new Vector3(direction.x, direction.y, myRB.velocity.z);
    }
}