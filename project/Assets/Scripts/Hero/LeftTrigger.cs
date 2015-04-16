using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MoveTrigger))]

public class LeftTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseExit(){
		GetComponent<MoveTrigger>().setMoving(false);
	}
}
