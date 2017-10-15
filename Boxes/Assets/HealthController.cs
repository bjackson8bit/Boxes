using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

	public GameObject hp1,hp2,hp3,hp4,hp5,hp6;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	internal void SetHP(int hp){
		Debug.Log ("hi");
		hp1.SetActive (false);
		hp2.SetActive (false);
		hp3.SetActive (false);
		hp4.SetActive (false);
		hp5.SetActive (false);
		hp6.SetActive (false);
		if (hp > 0) {
			hp6.SetActive (true);
		}
		if (hp > 1) {
			hp5.SetActive (true);
		}
		if (hp > 2) {
			hp4.SetActive (true);
		}
		if (hp > 3) {
			hp3.SetActive (true);
		}
		if (hp > 4) {
			hp2.SetActive (true);
		}
		if (hp > 5) {
			hp1.SetActive (true);
		}
	}
}
