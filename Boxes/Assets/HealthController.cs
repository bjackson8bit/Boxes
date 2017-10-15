using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

	public GameObject hp1,hp2,hp3;

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
		if (hp > 0) {
			hp1.SetActive (true);
		}
		if (hp > 1) {
			hp2.SetActive (true);
		}
		if (hp > 2) {
			hp3.SetActive (true);
		}
	}
}
