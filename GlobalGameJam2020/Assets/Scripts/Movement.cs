using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    public static event Action<float> OnChangeChanged = delegate {};

    [SerializeField] private float movementSpeed;
    [SerializeField] private float startMovementSpeed = 1000;
    [SerializeField] private float maxMovementSpeed;
    [SerializeField] private Vector3 playerBoundaries = new Vector3(0.9f, 0.9f, 0.9f);
    public float movementTimer;
    private Rigidbody myRB;
    private Camera main;

    void Awake() {
        myRB = GetComponent<Rigidbody>();
        main = Camera.main;
    }

    bool moving = false;
    void Update() {
        moving = MovementUpdate();

        if (moving == true && movementTimer - Time.deltaTime * 2 < 0 && movementTimer > 0) {
            movementTimer -= 1;
        }

        if (movementTimer < maxMovementSpeed && moving == false || movementTimer <= 0) {
            movementTimer += Time.deltaTime * 2;
        }

        Vector3 screenPosition = main.ViewportToWorldPoint(playerBoundaries);
        Debug.LogError(main.ViewportToWorldPoint(playerBoundaries));

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -screenPosition.x, screenPosition.x), Mathf.Clamp(transform.position.y, -screenPosition.y, screenPosition.y), 0);

        if (transform.position.x == -playerBoundaries.x || transform.position.x == playerBoundaries.x) {
            myRB.velocity = new Vector3(0, myRB.velocity.y, myRB.velocity.z);
        }
        if (transform.position.y == -playerBoundaries.y || transform.position.y == playerBoundaries.y) {
            myRB.velocity = new Vector3(myRB.velocity.x, 0, myRB.velocity.z);
        }

        OnChangeChanged(movementTimer / maxMovementSpeed);
    }

    bool hasMoved;
    public bool MovementUpdate() {
        hasMoved = false;
        if (movementTimer > 0) {
            if (Input.GetAxis("Horizontal") > 0.05f) {
                myRB.AddForce(Vector3.right * movementSpeed);
                movementTimer -= Time.deltaTime;
                hasMoved = true;
            } else if (Input.GetButtonDown("Horizontal") && Input.GetAxis("Horizontal") > 0) {
                myRB.AddForce(Vector3.right * startMovementSpeed);
                hasMoved = true;
            }

            if (Input.GetAxis("Horizontal") < -0.05f) {
                myRB.AddForce(Vector3.left * movementSpeed);
                movementTimer -= Time.deltaTime;
                hasMoved = true;
            } else if (Input.GetButtonDown("Horizontal") && Input.GetAxis("Horizontal") < 0) {
                myRB.AddForce(Vector3.left * startMovementSpeed);
                hasMoved = true;
            }

            if (Input.GetAxis("Vertical") > 0.05f) {
                myRB.AddForce(Vector3.up * movementSpeed);
                movementTimer -= Time.deltaTime;
                hasMoved = true;
            } else if (Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") > 0) {
                myRB.AddForce(Vector3.up * startMovementSpeed);
                hasMoved = true;
            }

            if (Input.GetAxis("Vertical") < -0.05f) {
                myRB.AddForce(Vector3.down * movementSpeed);
                movementTimer -= Time.deltaTime;
                hasMoved = true;
            } else if (Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") < 0) {
                myRB.AddForce(Vector3.down * startMovementSpeed);
                hasMoved = true;
            }

        }
        return hasMoved;
    }
}