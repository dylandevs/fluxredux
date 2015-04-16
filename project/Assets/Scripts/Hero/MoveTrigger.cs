using UnityEngine;
using System.Collections;

public class MoveTrigger : MonoBehaviour {

	private bool moving = false;

	void OnMouseDown(){
		moving = true;
		//print ("Hello " + this.name.ToString());
	}

	public bool isMoving(){
		return moving;
	}

	public void setMoving(bool move){
		moving = move;
	}
}
