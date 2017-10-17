using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	bool gameEnded = false;
	bool roundEnded = false;

	void Start() {
		gameEnded = false;
		roundEnded = false;
	}

	void Update () {
		if (!gameEnded && roundEnded) {
			int c = SceneManager.GetActiveScene ().buildIndex;
			if (c < SceneManager.sceneCountInBuildSettings) {
				SceneManager.LoadScene (c + 1);
			}
		}
		if (gameEnded) {
			if (Input.GetKeyDown (KeyCode.R)) {
				ScoreRecord.P1score = 0;
				ScoreRecord.P2score = 0;
				SceneManager.LoadScene (0);
			}
		}
	}
		
	public void setGameEnd(bool b) {
		gameEnded = b;
	}

	public void setRoundEnd(bool b) {
		roundEnded = b;
	}
}
