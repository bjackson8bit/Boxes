using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {
	public GameObject p1,p2;
	public Text gameOver;

	GameManager gm;
	Text p1score, p2score;
	int p1games = 0;
	int p2games = 0;

	// Use this for initialization
	void Start () {
		gm = GameObject.FindGameObjectWithTag ("Manager").GetComponent<GameManager>();
		p1score = p1.GetComponent<PlayerController> ().hc.GetComponentInChildren<Text> ();
		p2score = p2.GetComponent<PlayerController> ().hc.GetComponentInChildren<Text> ();
		p1games = ScoreRecord.P1score;
		p2games = ScoreRecord.P2score;
		UpdateUI ();
		gameOver.enabled = false;
	}

	internal void GameOver(GameObject player){
		string p = LayerMask.LayerToName (player.layer);
		if (p.Equals ("Player1")) {
			p2games++;
		} else {
			p1games++;
		}
		ScoreRecord.P2score = p2games;
		ScoreRecord.P1score = p1games;
		if (p1games == 3) {
			gameOver.text = "Player 1 Won!";
			gameOver.enabled = true;
			gm.setGameEnd (true);
		}

		if (p2games == 3) {
			gameOver.text = "Player 2 Won!";
			gameOver.enabled = true;
			gm.setGameEnd (true);
		}
		gm.setRoundEnd (true);
	}

	void UpdateUI(){
		p1score.text = "" + p1games;
		p2score.text = "" + p2games;
	}
}
