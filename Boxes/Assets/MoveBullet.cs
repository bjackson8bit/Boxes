using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PhysicsController))]
public class MoveBullet : MonoBehaviour {
	public float speed = 6;
	public Vector2 velocity;
	PhysicsController phys;
	public float bounces = 6;

	const float skinWidth = .015f;
	BoxCollider collider;

	Vector2 direction;

	void Start () {
		collider = GetComponent<BoxCollider> ();
		direction = Vector2.up;
		direction.Normalize ();
		phys = gameObject.GetComponent<PhysicsController> ();
	}

	void FixedUpdate () {
		phys.Move(velocity * speed * Time.deltaTime);
		BounceOffWall ();
		CheckPlayerCollision ();
		if (bounces <= 0) {
			Destroy (this.gameObject);
		}
	}

	public void SetVelocity(Vector2 velocity) {
		this.velocity = velocity;
	}

	public void SetSpeed(float speed) {
		this.speed = speed;
	}

	void BounceOffWall() {
		Vector2 normal = Collision ();
		if (phys.collisions.above || phys.collisions.below) {
			velocity = new Vector2 (velocity.x, -velocity.y);
			bounces--;
		}

		if (phys.collisions.left || phys.collisions.right) {
			velocity = new Vector2 (-velocity.x, velocity.y);
			bounces--;	
		}
	}

	Vector2 Collision() {
		if(phys.collisions.above){
			return Vector2.down;
		}
		if(phys.collisions.below){
			return Vector2.up;
		}
		if(phys.collisions.right){
			return Vector2.left;
		}
		if(phys.collisions.left){
			return Vector2.right;
		}
		return Vector2.zero;
	}

	//Damages player if necessary
	void CheckPlayerCollision() {
		if (phys.collisions.interacted != null && phys.collisions.interacted.tag == "Player") {
			PlayerController pc = phys.collisions.interacted.GetComponent<PlayerController> ();
			pc.Damage (1);

			Destroy (this.gameObject);
		}
	}
}
