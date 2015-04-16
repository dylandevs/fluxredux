using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class HeroMovement : MonoBehaviour {
	
	public MoveTrigger 		buttonRight, 
							buttonLeft, 
							buttonUp, 
							buttonDown,
							buttonCrouch;

	public GameObject 		heroSprite;
	private Animator 		anim;
	//private Rigidbody2D		heroBody;

	private BoxCollider2D 	heroCollider;
	public StopCollision	frontCollision;
	
	public float 			speed = 1f, 
							startRotation = 180,
							jumpForce = 1f;
	private float			numOfColl = 0f;
	private Vector2     	colliderSize,
							colliderPosition;
	private bool			facingLeft = false,
							grounded = false;


	// Use this for initialization
	void Start () {
		anim = heroSprite.GetComponent<Animator> ();
		heroCollider = GetComponent<BoxCollider2D> ();
		//heroBody = GetComponent<Rigidbody2D> ();
		colliderSize = heroCollider.size;
		colliderPosition = heroCollider.offset;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.Space)) {
			buttonCrouch.setMoving(true);
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			buttonCrouch.setMoving(false);
		}

		if (buttonCrouch.isMoving()) {
			anim.SetBool("Crouching", true);
			if(heroCollider.size.y == 2 && grounded){
				colliderSize.y *= 0.5f;
				colliderPosition.y -= colliderSize.y * 0.5f;
				heroCollider.size = colliderSize;
				heroCollider.offset = colliderPosition;
			}
		}

		if (!buttonCrouch.isMoving()) {
			anim.SetBool ("Crouching", false);
			if(heroCollider.size.y == 1){
				colliderPosition.y += colliderSize.y *0.5f;
				colliderSize.y *= 2;
				heroCollider.size = colliderSize;
				heroCollider.offset = colliderPosition;
			}
		}
		if (buttonDown.isMoving()) {
			buttonRight.setMoving(false);
			buttonLeft.setMoving(false);
			buttonDown.setMoving(false);
			anim.SetBool("Running", false);
		}
		if (buttonLeft.isMoving()) {
			flipHero();
			buttonLeft.setMoving(false);
			buttonRight.setMoving(true);
		}
		if (buttonRight.isMoving()) {
			if(!buttonCrouch.isMoving() && !frontCollision.hasStopped()){
				heroRun ();
			}
			anim.SetBool("Running", true);
		}
		if (buttonUp.isMoving() || Input.GetKeyDown(KeyCode.W)) {
			jump();
		}
		if (!grounded) {
			anim.SetBool ("Jumping", true);
		}

		if (Input.GetKey (KeyCode.D)) {
			if(facingLeft){
				flipHero ();
			}
			if(!facingLeft && !frontCollision.hasStopped() && !buttonCrouch.isMoving()){
				heroRun ();
			}
			anim.SetBool("Running", true);
		}

		if (Input.GetKey (KeyCode.A)) {
			if(!facingLeft){
				flipHero ();
			}
			if(facingLeft && !frontCollision.hasStopped() && !buttonCrouch.isMoving()){
				heroRun ();
			}
			anim.SetBool("Running", true);
		}

		if (Input.GetKeyUp (KeyCode.A) || Input.GetKeyUp (KeyCode.D)) {
			anim.SetBool("Running", false);
		}

	}

	private void heroRun(){
		transform.position += transform.right * 1 * Time.deltaTime * speed;
	}

	private void flipHero(){
		transform.Rotate(0, 180, 0);
		buttonRight.transform.Rotate(0, 180, 0);
		buttonLeft.transform.Rotate(0, 180, 0);
		frontCollision.transform.Rotate(0, 0, 180);
		facingLeft = !facingLeft;
	}

	public bool isGrounded(){
		return grounded;
	}
	
	public void setGrounded(bool landed){
		grounded = landed;
	}
	
	public bool isFacingLeft(){
		return facingLeft;
	}
	
	public void setFacingLeft(bool left){
		facingLeft = left;
	}

	public void resetNumOfColl(){
		numOfColl = 0f;
	}

	private void jump(){
		if (grounded){
			GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpForce);
			buttonUp.setMoving(false);
			grounded = false;
		}
	}

	private void OnCollisionEnter2D(Collision2D coll) {
		anim.SetBool ("Jumping", false);
		grounded = true;
		buttonUp.setMoving(false);
		++numOfColl;

	}

	private void OnCollisionExit2D(Collision2D coll){
		--numOfColl;
		//print ("numOfColl: " + numOfColl);
		if (numOfColl <= 0) {
			grounded = false;
		}
	}
}
