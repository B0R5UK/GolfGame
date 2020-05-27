using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {
	[SerializeField]
	private GameObject ground; //gameObject Of ground
	[SerializeField]
	private  GameObject ball; //GameObject of Ball
	[SerializeField]
	private GameObject trajectoryDotPrefab; //Prefab of trajectory Dot
	[SerializeField]
	private float xSpeed = 0.1f; //How fast x will grow
	[SerializeField]
	private float ySpeed = 0.18f; //How fast y will grow
	[SerializeField]
	private float maxX = 12.5f; //sets maximum value of x so it doesnt go over bounds
	[SerializeField]
	private float maxY = 19.2f; //sets maximum value of y so it doesnt go over bounds
	private List<GameObject> GeneratedDots = new List<GameObject>(); //list of generated dots - used to destroy them

  	private readonly Vector2 gravity = new Vector2(0f, -10f); //gravity used to calculate dots
  	private Vector2 launchVector = new Vector2(0,0); //Vector to give ball after mouse goes up
  	private Vector2 startPos; //Starting position for dots is determined by ball starting position
	private Rigidbody2D rigidBody;
	private float x = 0.5f; //x of vector to launch
	private float y = 0.5f; //y of vector to launch
	private int howManyDots = 5; //how many dots should be shown
	private int i = 0; //iterator used for determining value of howManyDots
	private bool mouseOnBall = false; // checks if mouse is on ball
	private bool inHole = false; //checks if the ball hit the hole
	private bool launched = false; //check if player already launched the ball
	private bool nextLevel = false; //initiates loading of next level
	private bool over = false;

	public bool isOver(){ 
		return over;
	}

	public bool isNext(){
		return nextLevel;
	}

		void OnMouseDown(){ //when mouse is clicked on ball 
		mouseOnBall = true;
	}
	void OnMouseUp(){ //when mouse stops clicking on ball
		mouseOnBall = false;
		if(!launched){
			Launch();
		}else{
			DestroyDots();
		}
	}
	void Awake () { 
		rigidBody = GetComponent<Rigidbody2D> ();
		startPos = ball.transform.position; //use ball position to set starting position
	}
	
	void Update() {
		i++;
		if(Input.GetMouseButton(0)&&!launched&&mouseOnBall){
			if(x>=maxX || y>=maxY){
				Launch();
			}
			x = x + xSpeed;
			y = y + ySpeed;
			launchVector.x = x;
			launchVector.y = y;
			if(i%5 == 0){ 
				howManyDots++; //increases value of dots shown every 5 iterarions
			}
			DrawDots(howManyDots,0.2f);
		}

		if(rigidBody.velocity == new Vector2(0,0)&&inHole){ //waits until ball stops in hole
			nextLevel = true;
		}

		if(rigidBody.velocity == new Vector2(0,0)&&!inHole&&launched){ //waits until ball stops outside hole
			over = true;
		}
	}

	public void RestartBall(){ //restarts ball position and the variables
		if(over == true){
			xSpeed = 0.2f;
			ySpeed = 0.18f;
		}else{
			xSpeed = xSpeed + 0.05f;
			ySpeed = ySpeed + 0.05f;
		}
		x = 0.5f;
		y = 0.5f;
		launched = false;
		inHole = false;
		nextLevel = false;
		over = false;
		ball.transform.position = startPos;
		Physics2D.IgnoreCollision(ground.GetComponent<Collider2D>(),GetComponent<Collider2D>(),false);
	}



	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "PotHole"){
			//Debug.Log("Entered Collision with pothole!");
			Physics2D.IgnoreCollision(ground.GetComponent<Collider2D>(),GetComponent<Collider2D>()); //when ball triggers pot hole, disable collisions with ground
		 }
	}

	void OnCollisionStay2D(Collision2D col){
		if(col.gameObject.tag == "PotHoleDown"){
				inHole = true;
			}else{
				inHole = false;
			}
		
	}

	void OnTriggerExit2D(Collider2D col){
		if(col.gameObject.tag == "PotHole"){
			//Debug.Log("Exited Collision with pothole!");
			Physics2D.IgnoreCollision(ground.GetComponent<Collider2D>(),GetComponent<Collider2D>(),false); //when ball escapes from pot hole area, trigger collisions with ground on
		 }
	}

  	private void Launch(){ //launches ball with given vector
	  	DestroyDots();
		launched = true;
    	rigidBody.velocity = launchVector;
  	}

	private Vector2 CalculatePosition(float elapsedTime){  //calculates position of the ball after given elapsedTime 
    	return gravity * elapsedTime * elapsedTime * 0.5f +
               launchVector * elapsedTime + startPos;
  	}

	private void DrawDots(int howMany, float timeStep){//draws trajectory dots 
		DestroyDots();
		for (int i = 0; i < howMany; i++) {
			GameObject trajectoryDot = Instantiate (trajectoryDotPrefab);
			GeneratedDots.Add(trajectoryDot);
    		trajectoryDot.transform.position = CalculatePosition (timeStep * i);
		}
	}

	private void DestroyDots(){ //destroys all trajectory dots
		foreach(var dot in GeneratedDots){
			Destroy(dot);
		}
	}

}
