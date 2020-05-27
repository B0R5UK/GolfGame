using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	[SerializeField]
	private Button restartBut; //Restart button shown on game over screen
	[SerializeField]
	private BallController ballControl; //BallController link
	[SerializeField]
	private GameObject potHolePrefab; //Prefab of pothole
	[SerializeField]
	private float maxX = -9f; //maximum position of pothole to right
	[SerializeField]
	private float minX = 4.92f; //minimum position of pothole to left
	[SerializeField]
	private Text scoreText; //Text field with score
	[SerializeField]
	private Text scoreOverText; //Text field with current score on game over screen
	[SerializeField]
	private Text bestScoreText; //Text field with best score on game over screen
	[SerializeField]
	private GameObject gameOverScreen; //game over screen

 	private GameObject potHole; //saves pot hole game object
	private int bestscore = 0; //keeps best score
	private int score = 0; //keeps current score
	

	void Start () {
		restartBut.onClick.AddListener(clickedRestart);
		placePothole();
		scoreText.text = "Score: "+score;
	}
	
	void clickedRestart(){ //when restart button is clicked
		score = 0;
		scoreText.text = "Score: "+score;
		nextLevel();
	}

	void showGameOver(){ //shows game over screen with scores
		if(score > bestscore){
			bestscore = score;
		}
		gameOverScreen.SetActive(true);

		bestScoreText.text = "Best score: " + bestscore;
		scoreOverText.text = "Current Score: "+score;
	}

	void placePothole(){ //randomly places pot hole
		potHole = Instantiate (potHolePrefab);
    	potHole.transform.position = new Vector2(Random.Range(minX,maxX),-0.02f);
	}

	void restartGame(){ //restarts current scene - SPACE
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
	}

	void nextLevel(){ //loads next level
		gameOverScreen.SetActive(false);
		Destroy(potHole);
		ballControl.RestartBall();
		placePothole();
	}

	void Update(){
		
		if(Input.GetKeyDown(KeyCode.Space)){
			restartGame();
		}
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}

		if(ballControl.isNext() == true){
			score++;
			scoreText.text = "Score: "+score;
			nextLevel();
		}

		if(ballControl.isOver() == true){
			showGameOver();
		}

	}
}





