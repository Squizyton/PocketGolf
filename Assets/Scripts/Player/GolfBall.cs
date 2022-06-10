using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GolfBall : MonoBehaviour
{
    public Camera cam;

    [SerializeField] private BallState state;
    [SerializeField] private float hitForce;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float maxSpeed;
    [SerializeField] private bool aiming;
    private Vector3 direction;

    private float distance;

    // Update is called once per frame
    private void Update()
    {
        if (state != BallState.Moving)
            Aiming();
    }

    public void FixedUpdate()
    {
        if (state == BallState.Moving)
            Moving();
    }

    private void Aiming()
    {
        if (Input.GetMouseButton(0))
        {
            aiming = true;
            //Get the forward direction of the golfball by adding transform.position with the mouse position
            direction = transform.position - cam.ScreenToWorldPoint(Input.mousePosition);
            //Calculate the distance between the golfball and the mouse position
            distance = Vector3.Distance(transform.position, direction);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && aiming)
        {
            rb.AddForce(direction * (hitForce * distance), ForceMode2D.Impulse);
            state = BallState.Moving;
            aiming = false;
        }
    }

    private void Moving()
    {
        //If the golfball is slowed down enough, stop it
        if (!(rb.velocity.magnitude <= 0.4)) return;

        //clamp the velocity of the golfball to the max speed
        if(rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;
        
        rb.velocity = Vector2.zero;
        state = BallState.Idle;
    }

    private enum BallState
    {
        Idle,
        Moving
    }

    private void OnDrawGizmos()
    {
        if (aiming)
        {
            //Draw a line from the ball to the mouse position
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, direction * 2);
        }
    }
}