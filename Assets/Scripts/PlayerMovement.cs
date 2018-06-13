using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpeedSetting { Idle, Slow, Fast, Reverse }
public class PlayerMovement : MonoBehaviour {
    // General
    public static PlayerMovement player;
    public Transform gelatoContainer;
    [HideInInspector] public Rigidbody rb;
    public BoxCollider[] colliders;

    // Movement
    public float speed, maxSpeed, currentSpeed;
    public float deadZone;
    public float pitch_speed, yaw_speed, roll_speed;
    public bool rotating = false;
    [HideInInspector]public bool rolling = true;
    public SpeedSetting speedSetting;

    float pitch, yaw, roll;
    Vector3 vel, addedPos;
    Vector2 stickInput;
    Quaternion rot;

    // Health & shield
    public float shieldTimerAmount;
    public float startHealth, startShield;
    public float rechargeRate;
    [HideInInspector] public float health, shield;
    public ParticleSystem ps;

    float shieldTimer;
    
    // Dashing
    public float dashAmount;
    float remainingDash = 0;
   
    // Deflection
    float deflectionTimer = 0;
    Quaternion deflectedAngle;
    Vector3 deflectedPos;

    // Update saves
    bool dashRight = false, dashLeft = false;

	void Awake () {

        player = this;
        rb = GetComponent<Rigidbody>();

        colliders = GetComponents<BoxCollider>();
      
        health = startHealth;
        shield = startShield;
        speedSetting = SpeedSetting.Idle;
    }

    private void Update()
    {
        // Inputs
        stickInput = new Vector2(Input.GetAxis("Pitch"), Input.GetAxis("Yaw"));

        if(Input.GetButtonDown("AButton"))
        {
            if (speedSetting == SpeedSetting.Idle || speedSetting == SpeedSetting.Reverse) speedSetting = SpeedSetting.Slow;
            else if (speedSetting == SpeedSetting.Slow) speedSetting = SpeedSetting.Fast;
            else speedSetting = SpeedSetting.Slow;
        }

        if (Input.GetButtonDown("BButton"))
        {
            if (speedSetting == SpeedSetting.Idle) speedSetting = SpeedSetting.Reverse;
            else speedSetting = SpeedSetting.Idle;
        }

        // Switch between rolling and camera
        if (Input.GetButton("CameraSwitch"))
        {
            if (rolling)
                rolling = false;
            else
                rolling = true;
        }

        //Shield recharge
        if(shieldTimer < Time.timeSinceLevelLoad && shield < startShield)
        {
            float newAmount = shield + (Time.deltaTime * rechargeRate);

            if(newAmount < startShield)
            {
                shield = newAmount;
            } else
            {
                shield = startShield;
            }
        }

        if (remainingDash == 0)
        {
            dashLeft = Input.GetButtonDown("DashLeft");
            dashRight = Input.GetButtonDown("DashRight");
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.game_over)
        {
            if (shield <= 0 && shieldTimer < Time.timeSinceLevelLoad) shield = startShield;

            if(deflectionTimer > Time.timeSinceLevelLoad)
            {
                Vector3 tempPos = Vector3.Slerp(rb.position, rb.position + deflectedPos, 0.25f);
                rb.MovePosition(tempPos);
            }
            // Deadzone
            if (stickInput.magnitude <= deadZone)
            {
                stickInput = Vector2.zero;
            }
            else
            {
                stickInput = stickInput.normalized * ((stickInput.magnitude - deadZone) / (1 - deadZone));
            }

            if (speedSetting == SpeedSetting.Reverse) currentSpeed = -speed;
            else if (speedSetting == SpeedSetting.Idle) currentSpeed = 0;
            else if (speedSetting == SpeedSetting.Slow) currentSpeed = speed;
            else currentSpeed = maxSpeed;

            if (speedSetting == SpeedSetting.Fast && !ps.isPlaying) ps.Play();
            else if (speedSetting != SpeedSetting.Fast && ps.isPlaying) ps.Stop();

            pitch = stickInput.x * pitch_speed;
            yaw = stickInput.y * yaw_speed;

            if (rolling)
            {
                roll = Input.GetAxis("Roll") * roll_speed;
            } else
            {
                roll = 0;
            }

            if (GameManager.Instance.invert)
            {
                rot = Quaternion.Euler(pitch, yaw, -roll);
            } else
            {
                rot = Quaternion.Euler(-pitch, yaw, roll);
            }
            rb.MoveRotation(rb.rotation * rot);

            if (pitch == 0 && yaw == 0 && roll == 0)
            {
                rotating = true;
            }
            else
            {
                rotating = false;
            }
            // Dash
            if (dashRight && remainingDash == 0)
            {
                remainingDash = dashAmount;
            }

            if (dashLeft && remainingDash == 0)
            {
                remainingDash = -dashAmount;
            }

            if (remainingDash == 0)
            {
                rb.MovePosition(rb.position + (transform.forward * currentSpeed));
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

    public void TakeDamge(float amount)
    {
        shieldTimer = Time.timeSinceLevelLoad + shieldTimerAmount;

        if (shield > 0)
        {
            shield -= amount;
        }
        else
        {
            health -= amount;
            if (health <= 0)
            {
                GameManager.Instance.Death();
            }
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "Astro")
        {
            TakeDamge(Mathf.Abs(currentSpeed));
            Vector3 dir = coll.contacts[0].point - rb.position;
            dir = -dir.normalized;
            DeflectPlayer(dir);
        }
    }

    void DeflectPlayer(Vector3 reflectPos)
    {
        float scale = currentSpeed;

        Quaternion newAngle = Quaternion.Euler(rb.rotation.x * scale,
            rb.rotation.y * scale, rb.rotation.y * scale);

        deflectedPos = reflectPos * scale * 5;
        deflectionTimer = Time.timeSinceLevelLoad + 1;
        deflectedAngle = newAngle;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Projectile" || coll.gameObject.tag == "Laser")
        {
            Collider currentCol = coll.GetComponent<Collider>();
            if (currentCol == colliders[1])
            {
                GameManager.Instance.Indicator("Left");
            }
            else if (currentCol == colliders[2])
            {
                GameManager.Instance.Indicator("Right");
            }
            else if (currentCol == colliders[3])
            {
                GameManager.Instance.Indicator("Front");
            }
            else if (currentCol == colliders[4])
            {
                GameManager.Instance.Indicator("Back");
            }
            else if (currentCol == colliders[5])
            {
                GameManager.Instance.Indicator("Top");
            }
            else
            {
                GameManager.Instance.Indicator("Bottom");
            }
        }
    }
}
