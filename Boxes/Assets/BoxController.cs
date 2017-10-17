using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsController))]
public class BoxController: MonoBehaviour {
	[Range(1, 2)]
	public float mass;
	public int damageAmount;
	public MoveBullet bullet;

	internal Vector2 velocity;
	bool againstWall;
	bool canBounce;
	Vector2 wall;
	PhysicsController phys;

	void Start () {
		phys = gameObject.GetComponent<PhysicsController> ();
		velocity = Vector2.zero;
		wall = Vector2.zero;
		againstWall = false;
		canBounce = true;
	}

	void FixedUpdate () {
		Debug.Log (gameObject.name + velocity);
		Debug.Log (gameObject.name + canBounce);
		againstWall = CheckWallCollision (velocity);
		//move this box
		phys.Move (velocity * Time.deltaTime);
		//check collision. If it's a box, move it accordingly
		BoxController hitBox = CheckBoxCollision ();
		if (phys.collisions.above || phys.collisions.below || phys.collisions.right || phys.collisions.left) {
			if (hitBox != null) {
				float bcmass = hitBox.mass;
				Vector2 bcVel = hitBox.velocity;
				float bcVelMag = hitBox.velocity.magnitude;
				float dot = Vector2.Dot (bcVel, this.velocity);
				if (mass < bcmass && bcVelMag == 0) {
					canBounce = true;
					//box I hit is massive and stationary: I stop
					velocity = Vector2.zero;
				} else if (mass < bcmass && bcVelMag != 0) {
					canBounce = true;
					if (dot <= 0) {
						float velX = velocity.x != 0 ? Mathf.Sign (velocity.x) : 0;
						float velY = velocity.y != 0 ? Mathf.Sign (velocity.y) : 0;
						Vector2 bulletDir = new Vector2 (velX, velY);
						float bcVelX = Mathf.Sign (bcVel.x);
						float bcVelY = Mathf.Sign (bcVel.y);
						bulletDir = new Vector2 (velX + bcVelX, velY + bcVelY);
						GameObject bulletInstance = GameObject.Instantiate (bullet.gameObject, gameObject.transform.position, gameObject.transform.rotation);
						bulletInstance.gameObject.GetComponent<MoveBullet> ().SetVelocity (bulletDir.normalized);
						bulletInstance.gameObject.GetComponent<MoveBullet> ().SetSpeed (Random.Range (5, 7));
						DestroyImmediate (gameObject);
					}
				} else if (mass >= bcmass && bcVelMag == 0 && !hitBox.againstWall) {
					//box I hit is weak and stationary: Keep moving together
					hitBox.Push (Collision (), velocity.magnitude * bcmass);
				} else if (mass > bcmass && bcVelMag != 0) {
					canBounce = true;
					//box I hit is weak and moving
					if (dot <= 0) {
						float velX = velocity.x != 0 ? Mathf.Sign (velocity.x) : 0;
						float velY = velocity.y != 0 ? Mathf.Sign (velocity.y) : 0;
						Vector2 bulletDir = new Vector2 (velX, velY);
						float bcVelX = Mathf.Sign (bcVel.x);
						float bcVelY = Mathf.Sign (bcVel.y);
						bulletDir = new Vector2 (velX + bcVelX, velY + bcVelY);
						GameObject bulletInstance = GameObject.Instantiate (bullet.gameObject, hitBox.transform.position, hitBox.transform.rotation);
						bulletInstance.gameObject.GetComponent<MoveBullet> ().SetVelocity (bulletDir.normalized);
						bulletInstance.gameObject.GetComponent<MoveBullet> ().SetSpeed (Random.Range (5, 7));
						DestroyImmediate (hitBox.gameObject);
					}
				} else if (mass >= bcmass && hitBox.againstWall) {
					velocity = Vector2.zero;
					canBounce = true;
				} else if (mass == bcmass && dot < 0) {
					if (canBounce) {
						velocity = -velocity;
						hitBox.velocity = -bcVel;
						canBounce = !canBounce;
					} else {
						velocity = Vector2.zero;
					}
				}
			} else if (hitBox == null) {
				canBounce = true;
				velocity = Vector2.zero;
				wall = Collision ();
			} else {
				canBounce = true;
				Vector2 diff = velocity - velocity.magnitude * hitBox.wall;
				if(diff.magnitude == 0) {
					velocity = Vector2.zero;
				}
			}
		}
		CheckPlayerCollision ();
	}

	public void Push (Vector2 dir, float speed) {
		velocity = dir * speed / mass;	
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
			pc.Damage (damageAmount);
		}
	}
	//returns null if hit a non-box
	BoxController CheckBoxCollision() {
		//colliding with a box
		if (phys.collisions.collided != null && phys.collisions.collided.tag == "Box") {
			BoxController bc = phys.collisions.collided.GetComponent<BoxController> ();	
			return bc;
		}
		//not colliding with a box
		return null;
	}

	//returns true if hit wall
	bool CheckWallCollision(Vector2 velocity) {
		return phys.RaycastCheckWall (velocity);
	}
}
