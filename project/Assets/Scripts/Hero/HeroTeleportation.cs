using UnityEngine;
using System.Collections;

public class HeroTeleportation : MonoBehaviour {

	public MoveTrigger 		heroCrouch; //Says when the hero is crouching

	public GameObject		heroSprite; //Gives access to the animation component of the sprite

	public SpriteRenderer	circleArea; //Circle of the teleportation area around the player

	private bool 			teleporting = false, // Says when here is in the middle of teleporting
							selected = false; // Says if the player has been touched/clicked

	private Vector3 		newPosition, //Stores the position the user wishes to teleport too
							oldPosition; //Stores the players current position

	private Quaternion		oldAngle; // Stores the players original angle so it knows to rotate back upright

	public float 			speed = 1.0f, // Controls speed of the teleportation
							jumpForce = 100f; // Adds a bit of force upward after a teleport is complete 
											  // so that the player hangs in the air for a fraction of a second longer

	private float 			differenceX = 0f,	//Stores the difference in X distance between the old and new positions
							differenceY = 0f,	//Stores the difference in Y distance between the old and new positions
							angle;				//Stores the angle that the player will be teleporting in to reach their destination.

	private Rigidbody2D		heroBody;		// stores the physics properties of the player so that it can be altered (such as applying force)
	private BoxCollider2D	heroCollider;	// Stores how many and with what the player is colliding with

	private HeroMovement	heroMovement;	// Gives access to the players current movement so that it can be queried
	private Animator		anim;			// Controls the animations the player is performing

	// Use this for initialization
	void Start () {
		//initiallize the values that couldn't be initiallized before runtime
		heroBody = GetComponent<Rigidbody2D> ();
		heroCollider = GetComponent<BoxCollider2D> ();
		heroMovement = GetComponent<HeroMovement> ();
		anim = heroSprite.GetComponent<Animator> ();

		//hides the circle from view until the player is about to teleport
		hideCircle ();

	}

	//Actions that occur each frame
	void Update(){

		//checks to see if the player has selected the player to begin teleporting
		if ((Input.GetMouseButtonDown (0) || Input.GetKey(KeyCode.Space))&& heroCrouch.isMoving() && !teleporting) {
			selected = true;
			showCircle();
		}

		//If the player has been selected, the mouse is tracked to make sure
		//that it has not left the blue circle which designates max teleportation distance
		if (Input.GetMouseButton (0) && selected) {
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = 7.0f; //fixes mousePos z access to 7 to make sure it is in line with the camera.

			newPosition = Camera.main.ScreenToWorldPoint(mousePos);
			oldPosition = transform.position;

			//if the user has left the binding circle, the character begins to teleport
			//to where the user's finger left the circle
			if(Vector3.Distance(oldPosition, newPosition) > 7.9){
				heroCrouch.setMoving(false);
				selected = false;
				startTeleport();
			}
			
		}

		//If the player is selected and the mouse or finger is released, teleporation will begin
		if(Input.GetMouseButtonUp(0) && selected && !teleporting){
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = 7.0f;

			newPosition = Camera.main.ScreenToWorldPoint(mousePos);
			oldPosition = transform.position;
			//print ("Distance: " + Vector3.Distance(oldPosition, newPosition));

			heroCrouch.setMoving(false); // removes the crouching animation of the player.
			selected = false;

			//If the mouse or finger was released over the hero teleporation does not occur,
			//otherwise teleportation occurs where the user released their mouse or finger
			if(Vector3.Distance(oldPosition, newPosition) > 5.1){
				startTeleport();
			}else{
				hideCircle();
			}
		}

		//If user was holding down the space key and released it before
		//selecting somewhere, the teleportation will be canceled
		if (Input.GetKeyUp (KeyCode.Space) && selected) {
			selected = false;
			hideCircle();
		}

		//Checks to see if the HeroTeleport animation has initiated 
		//in startTeleport() before teleporting movement occurs
		if (!teleporting) {
			if(anim.GetCurrentAnimatorStateInfo(0).IsName("HeroTeleport")){
				teleporting = true;
				//rotates the animation based on the angle, 
				//and flips it depending on which direction the player is facing
				heroSprite.transform.Rotate(0, ((heroMovement.isFacingLeft())?180:0), angle);
			}
		}

		//
		if (teleporting) {
			//print (anim.GetCurrentAnimationClipState(0));
			//transform.position = Vector3.Lerp(oldPosition, newPosition, speed * Time.deltaTime);

			//As the player gets closer to it's destination, if it gets close enough,
			//teleporation stops which is represented in this if statement
			if(Mathf.Abs(differenceX) > 0.02 && Mathf.Abs(differenceY) > 0.01){

				//moves the character to the right or left based on the difference of X over time.
				//deltaTime refers to the time since the last frame as frames don't occur a regular times
				//this helps to calculate how to compensate for skipped frames and cover the appropraite distance
				transform.position += transform.right * differenceX * speed * Time.deltaTime * ((heroMovement.isFacingLeft())?-1:1);

				//moves the character up or down based on the difference of Y over time.
				transform.position += transform.up * differenceY * speed * Time.deltaTime;

				//Removes the distance travelled from the total differences in distance
				//this keeps track how close the user is to reaching the final destination
				differenceX -= differenceX*speed*Time.deltaTime;
				differenceY -= differenceY*speed*Time.deltaTime;
				print ("differenceX = " + differenceX + "\ndifferenceY = " + differenceY);

			}else{
				//resets the values so that the user is no longer teleporting
				teleporting = false;
				heroBody.gravityScale = 1f;
				heroSprite.transform.rotation = oldAngle;
				heroCollider.enabled = true;
				anim.SetBool("Teleporting", false);
				hideCircle();

				//if the user teleported into the air, a bit of force is added upward
				//to give them a bit of hang time instead of instantly falling making
				//the player hard to select
				if(!heroMovement.isGrounded()){
					GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpForce);
				}
			}
		}
	}

	//Sets up the requirements of teleporation movement, before
	//the player may begin to teleport
	private void startTeleport(){

		differenceX = newPosition.x - oldPosition.x;
		differenceY = newPosition.y - oldPosition.y;

		// calculates the angle between the 
		// old position and new position from the ground
		angle = Mathf.Atan2(differenceY, differenceX) * 180 / Mathf.PI;
		oldAngle = heroSprite.transform.rotation;
		//print ("Facing Left:" + heroMovement.isFacingLeft());
		//print ("angle: " + angle);

		//turns gravity off while teleporting so no archs occur
		heroBody.gravityScale = 0f;

		// tells the player that it isn't touching the floor anymore
		// and that it is no longer colliding with anything
		heroMovement.setGrounded(false);
		heroMovement.resetNumOfColl();

		//begins the teleportation animation (which frame it actually
		//begins at is another question take care of in the update above)
		anim.SetBool("Teleporting", true);

		//turns collisions off while teleporting
		heroCollider.enabled = false;
	}

	//makes alph 0
	private void hideCircle(){
		circleArea.color = new Color (1, 1, 1, 0);
	}

	//makes alpha 1 for 100%
	private void showCircle(){
		circleArea.color = new Color (1, 1, 1, 1);
	}
}
