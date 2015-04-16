using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]

public class MakeSpriteInvisible : MonoBehaviour {

	//private SpriteRenderer
	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
