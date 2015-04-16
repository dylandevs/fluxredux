using UnityEngine;
using System.Collections;

public class CameraOrientation : MonoBehaviour {

	private float 	size,
					orgSize;
	private Rect	newRect,
					orgRect;
	// Use this for initialization
	void Start () {
		orgSize = Camera.main.orthographicSize;
		orgRect = Camera.main.rect;
		size = orgSize * 1.5f;
		newRect = orgRect;
		newRect.height *= 3;
	}
	
	// Update is called once per frame
	void Update () {
		if (Screen.orientation == ScreenOrientation.Portrait) {
			Camera.main.orthographicSize = size;
			Camera.main.rect = newRect;
		}else{
			Camera.main.orthographicSize = orgSize;
			Camera.main.rect = orgRect;
		}
	}
}
