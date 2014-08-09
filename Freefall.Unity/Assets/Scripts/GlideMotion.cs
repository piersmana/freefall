﻿using UnityEngine;
using System.Collections;

public class GlideMotion : MonoBehaviour {
	public float maxGlideSpeed = 80f;
	public float glideAcceleration = 10f;
	public float glideDeceleration = 3f;

	private PlayerController player;
	private PlayerGravity playerGravity;
	private PlayerWindMotion playerWindMotion;

	void Awake () {
		player = GetComponent<PlayerController>();
		playerGravity = GetComponent<PlayerGravity>();
		playerWindMotion = GetComponent<PlayerWindMotion>();
	}

	public void HandleGlideInput() {
		if(Input.GetButtonDown("A")) { // If player is not grounded, then the "A" button activates glide mode.
			if(!player.Gliding) {
				ActivateGlide();
			} else {
				DeactivateGlide();
			}
		}
	}

	public void ActivateGlide() {
		playerGravity.DisableGravity();
		player.Gliding = true;
	}

	public void DeactivateGlide() {
		player.Gliding = false;
	}

	public void HandleGlideMovement() {
		Vector2 movement = rigidbody2D.velocity;

		// Accelerate on any axis receiving input.
		if(Input.GetAxis("X-Axis") < 0 && rigidbody2D.velocity.x > -maxGlideSpeed) {
			movement.x -= glideAcceleration;
		}
		if(Input.GetAxis("X-Axis") > 0 && rigidbody2D.velocity.x < maxGlideSpeed) {
			movement.x += glideAcceleration; 
		}
		if(Input.GetAxis("Y-Axis") > 0 && rigidbody2D.velocity.y < maxGlideSpeed) {
			movement.y += glideAcceleration;
		}
		if(Input.GetAxis("Y-Axis") < 0 && rigidbody2D.velocity.y > -maxGlideSpeed) {
			movement.y -= glideAcceleration; 
		}

		// Decelerate on any axis receiving no input.
		if(Input.GetAxis("X-Axis") == 0) {
			if(movement.x < 0) {
				movement.x += glideDeceleration;
				if(movement.x > 0) {
					movement.x = 0;
				}
			}
			if(movement.x > 0) {
				movement.x -= glideDeceleration;
				if(movement.x < 0) {
					movement.x = 0;
				}
			}
		}
		if(Input.GetAxis("Y-Axis") == 0) {
			if(movement.y < 0) {
				movement.y += glideDeceleration;
				if(movement.y > 0) {
					movement.y = 0;
				}
			}
			if(movement.y > 0) {
				movement.y -= glideDeceleration;
				if(movement.y < 0) {
					movement.y = 0;
				}
			}
		}

		playerWindMotion.ApplyWindEffect(ref movement);

        EnforceMaximumSpeed(ref movement);

		rigidbody2D.velocity = movement;
	}

    private void EnforceMaximumSpeed(ref Vector2 movement) {
        if(movement.x > maxGlideSpeed) {
            movement.x = maxGlideSpeed;
        }
        if(movement.x < -maxGlideSpeed) {
            movement.x = -maxGlideSpeed;
        }
        if(movement.y > maxGlideSpeed) {
            movement.y = maxGlideSpeed;
        }
        if(movement.y < -maxGlideSpeed) {
            movement.y = -maxGlideSpeed;
        }
    }
}
