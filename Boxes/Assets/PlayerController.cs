using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PhysicsController))]
public class PlayerController : MonoBehaviour {
	public float speed = 5f;
	public float pushPower = 5f;
	public bool playerOne = true;
	public GameObject uiparent;
	public int invincibilityFrames = 70;
	public ScoreController sc;
	int invincibilityCounter = 0;
	internal HealthController hc;

	public int baseHealth = 3;
	int health;
	bool moving = true;
	bool hurt = false;
	Vector2 movementDirection;
	PhysicsController phys;

	void Start() {
		phys = gameObject.GetComponent<PhysicsController> ();
		health = baseHealth;
		hc = uiparent.GetComponent<HealthController> ();
	}
		
	void FixedUpdate () {
		if (hurt) {
			invincibilityCounter++;
			if (invincibilityCounter % 5 == 0) {
				this.GetComponent<MeshRenderer> ().enabled = !this.GetComponent<MeshRenderer> ().enabled;
			}

			if (invincibilityCounter >= invincibilityFrames) {
				hurt = false;
				invincibilityCounter = 0;
				this.GetComponent<MeshRenderer> ().enabled = true;
			}
		}
		Vector2 input = Vector2.zero;
		if (playerOne) {
			input = new Vector2 (Input.GetAxisRaw ("Horizontal1"), Input.GetAxisRaw ("Vertical1"));
		} else {
			input = new Vector2 (Input.GetAxisRaw ("Horizontal2"), Input.GetAxisRaw ("Vertical2"));
		}
		movementDirection = input.normalized;

		Vector2 move = input.normalized;
		move = move * speed * Time.deltaTime;
		phys.Move (move);
		CheckBoxCollision ();
	}

	Vector2 FaceDirection() {
		float up = 0;
		float right = 0;
		up = Mathf.Abs(movementDirection.x) < Mathf.Abs(movementDirection.y) ? 1 * Mathf.Sign(movementDirection.y) : 0;
		right = Mathf.Abs(movementDirection.y) < Mathf.Abs(movementDirection.x) ? 1 * Mathf.Sign(movementDirection.x) : 0;
		return new Vector2 (right, up);
	}

	Vector2 Collision() {
		if(phys.collisions.above){
			return Vector2.up;
		}
		if(phys.collisions.below){
			return Vector2.down;
		}
		if(phys.collisions.right){
			return Vector2.right;
		}
		if(phys.collisions.left){
			return Vector2.left;
		}
		return Vector2.zero;
	}

	void CheckBoxCollision() {
		if (phys.collisions.collided != null && phys.collisions.collided.tag == "Box") {
			BoxController bc = phys.collisions.collided.GetComponent<BoxController> ();	
			if (bc.velocity.magnitude == 0) {
				bc.Push (Collision(), pushPower);
			}
		}
	}

	internal void Damage(int amt){
		if (!hurt) {
			hurt = true;
			health -= amt;
			hc.SetHP (health);
			this.GetComponent<AudioSource> ().Play ();
		}
		
		if (health <= 0) {
			sc.GameOver(gameObject);
		}
	}
}
