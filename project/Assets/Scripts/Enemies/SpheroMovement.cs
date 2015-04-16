using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpheroMovement : MonoBehaviour {

	private Vector3		startPoint;
	public float		speed = 1,
						distance = 1;
	private float		distanceTravelled = 0;
	public bool			facingLeft = true;



	// Use this for initialization
	void Start () {
		startPoint = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.right * Time.deltaTime * speed * ((facingLeft)? -1:1);
		distanceTravelled += Time.deltaTime * speed;

		if (distance < distanceTravelled) {
			changeDirections();
		}
	}

	void changeDirections(){
		facingLeft = !facingLeft;
		distanceTravelled = 0;
	}

	private void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "floor") {
			changeDirections ();
		}
	}
}
