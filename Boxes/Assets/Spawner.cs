using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	public GameObject toSpawn;
	public MoveBullet bullet;
	int count = 0;
	public int spawnTime;
	public int timer;

	void FixedUpdate () {
		if (count == 0) {
			timer++;
		}

		if (timer >= spawnTime && count == 0) {
			GameObject box = (GameObject) GameObject.Instantiate (toSpawn, transform.position, transform.rotation);
			box.GetComponent<BoxController> ().bullet = bullet;
			timer = 0;
			if(spawnTime > 100){
				spawnTime -= 10;
			}
		}
		
	}

	void OnTriggerEnter(Collider other) {
		count++;		
	}

	void OnTriggerExit(Collider other) {
		count--;
	}
}
