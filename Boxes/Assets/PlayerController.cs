using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float speed = 10f;
	public float tileMultip = 1;
	public LayerMask obstacleMask;
	public int bufferTimeout;
	Vector3 targetPos;
	Vector3 currentPos;
	bool reachedPos = true;
	string faceDir = "";
	string bufferDir = "";
	int bufferTime = 0;



	void Start() {
		targetPos = transform.position;
		faceDir = "";
	}

	// Update is called once per frame
	void FixedUpdate () {
		print (bufferDir);
		if(bufferTime > 0){
			bufferTime--;
		}
		else{
			bufferDir = "";
		}

		Vector2 input = new Vector2(Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw("Vertical"));
		string nextDir = "";
		bool onTile = transform.position == targetPos;

		if (input.x > 0 && !faceDir.Equals("r")) {
			nextDir = "r";
		}
		else if (input.x < 0 && !faceDir.Equals("l")) {
			nextDir = "l";
		}

		else if (input.y > 0 && !faceDir.Equals("u")) {
			nextDir = "u";
		}

		else if (input.y < 0 && !faceDir.Equals("d")) {
			nextDir = "d";
		}

		if (!nextDir.Equals ("")) {
			bufferDir = nextDir;
			bufferTime = bufferTimeout;
		}		

		if (onTile) {
			print ("Kek1");
			faceDir = "";
			reachedPos = true;
			switch(bufferDir){
			case "r":
				MovePlayerRight ();
				break;
			case "l":
				MovePlayerLeft ();
				break;
			case "u":
				MovePlayerUp ();
				break;
			case "d":
				MovePlayerDown ();
				break;
			
			}
			bufferDir = "";
		}
		if (!onTile) {
			transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
		}

		//RotatePlayer ();
	}

	bool RaycastCheck(Vector3 direction) {
		float rayLength = 0.01f + tileMultip;
		Vector3 rayOrigin = transform.position;
		Debug.DrawLine (rayOrigin, rayOrigin + direction * (rayLength));
		if (Physics.Raycast (rayOrigin, direction, rayLength, obstacleMask)) {
			return true;
		} else {
			return false;
		}
	}

	void RotatePlayer() {
		switch (faceDir) {
		case "u":
			transform.eulerAngles = new Vector3 (0, 0, 0);
			break;
		case "d":
			transform.eulerAngles = new Vector3 (0, 0, 180);
			break;
		case "l":
			transform.eulerAngles = new Vector3 (0, 0, 90);
			break;
		case "r":
			transform.eulerAngles = new Vector3 (0, 0, -90);
			break;
		default:
			break;
		}
	}

	public void MovePlayerLeft() {
		if (!RaycastCheck (Vector3.left) && reachedPos) {
			targetPos += Vector3.left * tileMultip;
			reachedPos = false;
		}
		faceDir = "l";	
	}

	public void MovePlayerRight() {
		if (!RaycastCheck (Vector3.right) && reachedPos) {
			targetPos += Vector3.right * tileMultip;
			reachedPos = false;
		}
		faceDir = "r";
	}

	public void MovePlayerUp() {
		if (!RaycastCheck (Vector3.up) && reachedPos) {
			targetPos += Vector3.up * tileMultip;
			reachedPos = false;
		}
		faceDir = "u";
	}

	public void MovePlayerDown() {
		if (!RaycastCheck (Vector3.down) && reachedPos) {
			targetPos += Vector3.down * tileMultip;
			reachedPos = false;
		}
		faceDir = "d";
	}


}
