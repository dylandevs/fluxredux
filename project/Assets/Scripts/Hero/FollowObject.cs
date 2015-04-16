using UnityEngine;
using System.Collections;

public class FollowObject : MonoBehaviour {

	public GameObject followedObject;
	private Vector3 follow;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		follow = transform.position;
		follow.x = followedObject.transform.position.x;
		follow.y = followedObject.transform.position.y;

		transform.position = follow;
	}
}
