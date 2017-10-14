using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsController))]
public class BoxController: MonoBehaviour {
	internal Vector2 velocity;
	Vector2 wall;
	PhysicsController phys;
	public float mass;

	void Start () {
		phys = gameObject.GetComponent<PhysicsController> ();
		velocity = Vector2.zero;
		wall = Vector2.zero;
	}

	void FixedUpdate () {
		
		phys.Move (velocity * Time.deltaTime);
		BoxController hitBox = CheckBoxCollision ();
		Debug.Log (hitBox);
		if (phys.collisions.above || phys.collisions.below
		   || phys.collisions.right || phys.collisions.left) {
			//Equal boxes inelastically collide (separate)
			if (hitBox != null && hitBox.mass >= mass) {
				velocity = Vector2.zero;
			} else if (hitBox == null) {
				velocity = Vector2.zero;
				wall = Collision ();
			} else {
				Vector2 diff = velocity - velocity.magnitude * hitBox.wall;
				if(diff.magnitude == 0) {
					velocity = Vector2.zero;
				}
			}

		}

	}

	public void Push (Vector2 dir, float speed) {
		velocity = dir * speed * mass;	
	}

	internal Vector2 Collision() {
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

	//returns null if hit a non-box
	BoxController CheckBoxCollision() {
		if (phys.collisions.collided != null && phys.collisions.collided.tag == "Box") {
			BoxController bc = phys.collisions.collided.GetComponent<BoxController> ();	
			if (bc.velocity.magnitude == 0 && this.mass >= bc.mass) {
				bc.Push (Collision(), velocity.magnitude);
			}
			return bc;
		}
		return null;
	}
}
