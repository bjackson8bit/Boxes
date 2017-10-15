using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsController))]
public class BoxController: MonoBehaviour {
	internal Vector2 velocity;
	Vector2 wall;
	PhysicsController phys;
	public float mass;
	public MoveBullet bullet;


	void Start () {
		phys = gameObject.GetComponent<PhysicsController> ();
		velocity = Vector2.zero;
		wall = Vector2.zero;
	}

	void FixedUpdate () {
		
		phys.Move (velocity * Time.deltaTime);
		BoxController hitBox = CheckBoxCollision ();
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

		CheckPlayerCollision ();

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

	//Damages player if necessary
	void CheckPlayerCollision() {
		if (phys.collisions.interacted != null && phys.collisions.interacted.tag == "Player") {
			PlayerController pc = phys.collisions.interacted.GetComponent<PlayerController> ();
			pc.Damage (1);
		}
	}
	//returns null if hit a non-box
	BoxController CheckBoxCollision() {
		if (phys.collisions.collided != null && phys.collisions.collided.tag == "Box") {
			BoxController bc = phys.collisions.collided.GetComponent<BoxController> ();	
			float bcmass = bc.mass;
			float bcVelMag = bc.velocity.magnitude;
			if (bcVelMag == 0 && this.mass >= bcmass) {
				bc.Push (Collision (), velocity.magnitude / bcmass);
			} else if (bcVelMag != 0 && this.mass > bcmass) {
				Vector2 velocity = Random.insideUnitCircle;
				GameObject bulletInstance = GameObject.Instantiate (bullet.gameObject, bc.transform.position, bc.transform.rotation);
				bulletInstance.gameObject.GetComponent<MoveBullet>().SetVelocity(velocity.normalized);
				bulletInstance.gameObject.GetComponent<MoveBullet>().SetSpeed (Random.Range (5, 7));
				Destroy (bc.gameObject);

			} 
			return bc;
		}
		return null;
	}
}
