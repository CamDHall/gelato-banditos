using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public static PlayerMovement player;
    public int health;
    public float thrustSpeed;

    public float deadZone;

    public float pitch_speed, yaw_speed, roll_speed;

    public bool rotating = false;

    public float pitch, yaw, roll;
    Vector3 vel;

    public float accelRate, deccelTime, maxSpeed;
    public float acceleration = 0;

    // Dashing
    [SerializeField] float remainingDash = 0;
    public float dashAmount;

    public Rigidbody rb;

    public BoxCollider[] colliders;

	void Awake () {
        accelRate *= 10;

        player = this;
        rb = GetComponent<Rigidbody>();

        colliders = GetComponents<BoxCollider>();

        accelRate = thrustSpeed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.game_over)
        {
            // Deadzone
            Vector2 stickInput = new Vector2(Input.GetAxis("Pitch"), Input.GetAxis("Yaw"));
            if (stickInput.magnitude <= deadZone)
            {
                stickInput = Vector2.zero;
            }
            else
            {
                stickInput = stickInput.normalized * ((stickInput.magnitude - deadZone) / (1 - deadZone));
            }

            if (Input.GetAxis("Thrust") != 0)
            {
                if (acceleration < maxSpeed)
                {
                    acceleration += (accelRate * Time.deltaTime);
                }
            }
            else
            {
                if (acceleration > 0)
                {
                    if (acceleration < 0.01f)
                    {
                        acceleration = 0;
                    }
                    else
                    {
                        acceleration = Mathf.Lerp(acceleration, 0, deccelTime * Time.deltaTime);
                    }
                }
            }

            pitch = stickInput.x * pitch_speed;
            yaw = stickInput.y * yaw_speed;
            //roll = Input.GetAxis("Roll") * roll_speed;

            transform.Rotate(pitch, yaw, 0);

            if (pitch == 0 && yaw == 0 && roll == 0)
            {
                rotating = true;
            }
            else
            {
                rotating = false;
            }

            // Dash
            if (Input.GetButtonDown("DashRight"))
            {
                if (remainingDash == 0)
                {
                    remainingDash = dashAmount;
                }
            }
            if (Input.GetButtonDown("DashLeft"))
            {
                if (remainingDash == 0)
                {
                    remainingDash = -dashAmount;
                }
            }

            if (remainingDash == 0)
            {
                rb.MovePosition(rb.position + (transform.forward * acceleration));
            }
            else
            {
                remainingDash = Mathf.Lerp(remainingDash, 0, 0.2f);
                Vector3 vDash = rb.position + (transform.right * remainingDash);

                rb.MovePosition(vDash);

                if (Mathf.Abs(remainingDash) < 0.1f)
                {
                    remainingDash = 0;
                }
            }
        }
    }

    public void TakeDamge(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            GameManager.Instance.Death();
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "Astro")
        {
            GameManager.Instance.Death();
        }
    }
}
