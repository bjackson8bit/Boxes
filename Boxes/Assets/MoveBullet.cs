using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PhysicsController))]
public class MoveBullet : MonoBehaviour {
	public float speed = 6;
	public Vector2 velocity;
	PhysicsController phys;

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
	}

	void SetVelocity(Vector2 velocity) {
		this.velocity = velocity;
	}

	void BounceOffWall() {
		Vector2 normal = Collision ();
		if (phys.collisions.above || phys.collisions.below) {
			velocity = new Vector2 (velocity.x, -velocity.y);
		}

		if (phys.collisions.left || phys.collisions.right) {
			velocity = new Vector2 (-velocity.x, velocity.y);
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
}
