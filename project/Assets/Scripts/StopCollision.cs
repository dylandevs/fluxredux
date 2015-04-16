using UnityEngine;
using System.Collections;

public class StopCollision : MonoBehaviour {

	private bool stop = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//stop = false;
	}

	public bool hasStopped(){
		return stop;
	}

	public void setStop(bool shouldStop){
		stop = shouldStop;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "floor") {
			stop = true;
			//print ("collided");
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		stop = false;
	}
}
