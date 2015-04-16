using UnityEngine;
using System.Collections;

public class SlowTimeTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		Time.timeScale = 0.5f;
	}

	void OnMouseUp(){
		Time.timeScale = 1.0f;
	}
}
