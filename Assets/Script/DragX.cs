using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragX : MonoBehaviour {

	private Vector3 offset;
	private Vector3 screenPoint;
	private Vector3 currentScreenPoint;
	private Vector3 currentPosition;

	void Update () {

		if (-6.0f > transform.position.x) {

			transform.position = new Vector3(-6.0f, 1.0f, 0.0f);

		} else if (6.0f < transform.position.x) {

			transform.position = new Vector3(6.0f, 1.0f, 0.0f);

		}

	}


	void OnMouseDown()
	{

		screenPoint = Camera.main.WorldToScreenPoint(transform.position);


		float x = Input.mousePosition.x;

		offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(x, 1.0f, screenPoint.z));

	}


	void OnMouseDrag(){

		float x = Input.mousePosition.x;

		currentScreenPoint = new Vector3(x, 1.0f, screenPoint.z);

		currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + offset;

		transform.position = currentPosition;

	}

}
