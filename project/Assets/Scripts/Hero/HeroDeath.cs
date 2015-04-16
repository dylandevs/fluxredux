using UnityEngine;
using System.Collections;

public class HeroDeath : MonoBehaviour {

	Vector3	spawnPoint;

	// Use this for initialization
	void Start () {
		spawnPoint = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		print ("Position: " + transform.position.x + ", " + transform.position.y);

		if (transform.position.x < -21.5 || transform.position.y < 1.9 || transform.position.x > 51.8) {
			transform.position = spawnPoint;
		}
	}
}
